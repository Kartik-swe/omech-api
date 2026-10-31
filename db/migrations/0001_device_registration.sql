-- =====================================================================
-- 0001: Device registration (phone access from any network)
-- Runs automatically on deploy (see deploy/run-migrations.sh).
--
-- ADDITIONS ONLY - no existing table, procedure or data is changed.
--   new table     M_USER_DEVICE
--   new function  FN_IS_ADMIN_USER
--   new procs     IU_DEVICE_REQUEST, DT_DEVICE_VALIDATE,
--                 DT_USER_DEVICES, IU_DEVICE_ACTION
--
-- Rules (agreed 2026-10-29/31):
--   * only ADMIN users (USER_TYPE_SRNO = 1) can approve / reject / revoke / renew
--   * an approval is valid for 30 days, then the device must be renewed
--   * max 2 devices per user (pending + currently approved)
--
-- Idempotent: safe to run more than once.
-- =====================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ---------------------------------------------------------------------
-- 1) Table
-- ---------------------------------------------------------------------
IF OBJECT_ID(N'[dbo].[M_USER_DEVICE]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[M_USER_DEVICE](
        [DEVICE_SRNO]      [int] IDENTITY(1,1) NOT NULL,
        [USER_SRNO]        [int]           NOT NULL,   -- owner of the device
        [DEVICE_NAME]      [varchar](100)  NOT NULL,   -- e.g. 'Ramesh - iPhone'
        [DEVICE_KEY_HASH]  [char](64)      NOT NULL,   -- SHA-256 (hex) of the secret key; the key itself is never stored
        [DEVICE_STATUS]    [varchar](20)   NOT NULL,   -- PENDING / APPROVED / REJECTED / REVOKED  (EXPIRED is computed from EXPIRES_AT)
        [REQUEST_IP]       [varchar](45)   NULL,
        [USER_AGENT]       [varchar](500)  NULL,
        [REQUESTED_AT]     [datetime]      NOT NULL CONSTRAINT [DF_M_USER_DEVICE_REQUESTED_AT] DEFAULT (getdate()),
        [ACTION_BY]        [int]           NULL,       -- admin who last approved / rejected / revoked / renewed
        [ACTION_AT]        [datetime]      NULL,
        [EXPIRES_AT]       [datetime]      NULL,       -- set on approve / renew (+30 days)
        [LAST_SEEN_AT]     [datetime]      NULL,
        [LAST_SEEN_IP]     [varchar](45)   NULL,
        [DEL_FLAG]         [bit]           NOT NULL CONSTRAINT [DF_M_USER_DEVICE_DEL_FLAG] DEFAULT ((0)),
        [DEL_BY]           [int]           NULL,
        [DEL_AT]           [datetime]      NULL,
        [CREATED_BY]       [int]           NULL,
        [CREATED_AT]       [datetime]      NULL CONSTRAINT [DF_M_USER_DEVICE_CREATED_AT] DEFAULT (getdate()),
        [MODIFIED_BY]      [int]           NULL,
        [MODIFIED_AT]      [datetime]      NULL,
        CONSTRAINT [PK_M_USER_DEVICE] PRIMARY KEY CLUSTERED ([DEVICE_SRNO] ASC),
        CONSTRAINT [FK_M_USER_DEVICE_USER]      FOREIGN KEY ([USER_SRNO]) REFERENCES [dbo].[M_USER] ([USER_SRNO]),
        CONSTRAINT [FK_M_USER_DEVICE_ACTION_BY] FOREIGN KEY ([ACTION_BY]) REFERENCES [dbo].[M_USER] ([USER_SRNO]),
        CONSTRAINT [CK_M_USER_DEVICE_STATUS]    CHECK ([DEVICE_STATUS] IN ('PENDING','APPROVED','REJECTED','REVOKED'))
    ) ON [PRIMARY];

    CREATE UNIQUE NONCLUSTERED INDEX [UX_M_USER_DEVICE_KEY_HASH] ON [dbo].[M_USER_DEVICE] ([DEVICE_KEY_HASH]);
    CREATE NONCLUSTERED INDEX [IX_M_USER_DEVICE_USER] ON [dbo].[M_USER_DEVICE] ([USER_SRNO], [DEVICE_STATUS]);
END
GO

-- ---------------------------------------------------------------------
-- 2) Is this user an admin?  (active, not deleted, USER_TYPE_SRNO = 1)
-- ---------------------------------------------------------------------
CREATE OR ALTER FUNCTION [dbo].[FN_IS_ADMIN_USER] (@USER_SRNO INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE WHEN EXISTS (
        SELECT 1
        FROM M_USER MU
        WHERE MU.USER_SRNO = @USER_SRNO
          AND MU.IS_ACTIVE = 1
          AND MU.DEL_FLAG = 0
          AND MU.USER_TYPE_SRNO = 1          -- 1 = Admin
    ) THEN 1 ELSE 0 END;
END
GO

-- ---------------------------------------------------------------------
-- 3) A phone asks for access (called from the "Request access" screen).
--    Checks username/password exactly like SP_LOGIN_USER (username
--    case-insensitive, password case-sensitive, active users only),
--    enforces max 2 devices, and creates a PENDING row.
--    RESULT_CODE: 1 = request created, 0 = invalid credentials,
--                 2 = device limit reached
-- ---------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[IU_DEVICE_REQUEST]
    @USERNAME        NVARCHAR(50),
    @PASSWORD        NVARCHAR(255),
    @DEVICE_NAME     VARCHAR(100),
    @DEVICE_KEY_HASH CHAR(64),
    @REQUEST_IP      VARCHAR(45)  = NULL,
    @USER_AGENT      VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @USER_SRNO INT, @STORED_PASSWORD NVARCHAR(255);

    SELECT @USER_SRNO = USER_SRNO, @STORED_PASSWORD = PASSWORD
    FROM M_USER
    WHERE USERNAME COLLATE SQL_Latin1_General_CP1_CI_AS = @USERNAME
      AND IS_ACTIVE = 1
      AND DEL_FLAG = 0;

    IF @USER_SRNO IS NULL
       OR @STORED_PASSWORD IS NULL
       OR @STORED_PASSWORD COLLATE SQL_Latin1_General_CP1_CS_AS <> @PASSWORD
    BEGIN
        SELECT 0 AS RESULT_CODE, 'Invalid username or password.' AS RESULT_MSG;
        RETURN;
    END

    -- max 2 devices: pending requests + currently valid approvals
    IF (SELECT COUNT(*) FROM M_USER_DEVICE
        WHERE USER_SRNO = @USER_SRNO
          AND DEL_FLAG = 0
          AND (DEVICE_STATUS = 'PENDING'
               OR (DEVICE_STATUS = 'APPROVED' AND EXPIRES_AT > GETDATE()))) >= 2
    BEGIN
        SELECT 2 AS RESULT_CODE,
               'You already have 2 devices registered or waiting for approval. Ask an admin to revoke one first.' AS RESULT_MSG;
        RETURN;
    END

    INSERT INTO M_USER_DEVICE
        (USER_SRNO, DEVICE_NAME, DEVICE_KEY_HASH, DEVICE_STATUS, REQUEST_IP, USER_AGENT, REQUESTED_AT, CREATED_BY, CREATED_AT)
    VALUES
        (@USER_SRNO, LTRIM(RTRIM(@DEVICE_NAME)), @DEVICE_KEY_HASH, 'PENDING', @REQUEST_IP, LEFT(@USER_AGENT, 500), GETDATE(), @USER_SRNO, GETDATE());

    SELECT 1 AS RESULT_CODE, 'Request sent. An admin must approve this device before you can log in.' AS RESULT_MSG,
           SCOPE_IDENTITY() AS DEVICE_SRNO;
END
GO

-- ---------------------------------------------------------------------
-- 4) Look up a device by its key hash (used on every request coming from
--    outside the allowed IPs, and by the phone's status screen).
--    IS_VALID = 1 only for APPROVED, not expired, owner active.
--    @TOUCH = 1 also records LAST_SEEN_AT / LAST_SEEN_IP for valid devices.
-- ---------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[DT_DEVICE_VALIDATE]
    @DEVICE_KEY_HASH CHAR(64),
    @IP              VARCHAR(45) = NULL,
    @TOUCH           BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NOW DATETIME = GETDATE();

    IF @TOUCH = 1
        UPDATE D
        SET LAST_SEEN_AT = @NOW, LAST_SEEN_IP = @IP
        FROM M_USER_DEVICE D
        INNER JOIN M_USER U ON U.USER_SRNO = D.USER_SRNO
        WHERE D.DEVICE_KEY_HASH = @DEVICE_KEY_HASH
          AND D.DEL_FLAG = 0
          AND D.DEVICE_STATUS = 'APPROVED'
          AND D.EXPIRES_AT > @NOW
          AND U.IS_ACTIVE = 1 AND U.DEL_FLAG = 0;

    SELECT
        D.DEVICE_SRNO,
        D.USER_SRNO,
        D.DEVICE_NAME,
        CASE WHEN D.DEVICE_STATUS = 'APPROVED' AND D.EXPIRES_AT <= @NOW THEN 'EXPIRED'
             ELSE D.DEVICE_STATUS END AS DEVICE_STATUS,
        D.EXPIRES_AT,
        CASE WHEN D.DEVICE_STATUS = 'APPROVED' AND D.EXPIRES_AT > @NOW
                  AND U.IS_ACTIVE = 1 AND U.DEL_FLAG = 0
             THEN 1 ELSE 0 END AS IS_VALID
    FROM M_USER_DEVICE D
    INNER JOIN M_USER U ON U.USER_SRNO = D.USER_SRNO
    WHERE D.DEVICE_KEY_HASH = @DEVICE_KEY_HASH
      AND D.DEL_FLAG = 0;
END
GO

-- ---------------------------------------------------------------------
-- 5) Admin list for Master > Devices.
--    Table  : RESULT_CODE (-1 = not admin, 1 = ok), RESULT_MSG
--    Table1 : devices (only when admin)
--    @DEVICE_STATUS: NULL = all, or PENDING / APPROVED / EXPIRED / REJECTED / REVOKED
-- ---------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[DT_USER_DEVICES]
    @USER_SRNO     INT,
    @DEVICE_STATUS VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.FN_IS_ADMIN_USER(@USER_SRNO) = 0
    BEGIN
        SELECT -1 AS RESULT_CODE, 'Only admin users can manage devices.' AS RESULT_MSG;
        RETURN;
    END

    SELECT 1 AS RESULT_CODE, 'Success' AS RESULT_MSG;

    DECLARE @NOW DATETIME = GETDATE();

    SELECT *
    FROM (
        SELECT
            D.DEVICE_SRNO,
            D.USER_SRNO,
            U.USERNAME,
            LTRIM(RTRIM(ISNULL(U.F_NAME, '') + ' ' + ISNULL(U.L_NAME, ''))) AS FULL_NAME,
            D.DEVICE_NAME,
            CASE WHEN D.DEVICE_STATUS = 'APPROVED' AND D.EXPIRES_AT <= @NOW THEN 'EXPIRED'
                 ELSE D.DEVICE_STATUS END AS DEVICE_STATUS,
            D.REQUESTED_AT,
            D.REQUEST_IP,
            D.USER_AGENT,
            D.EXPIRES_AT,
            D.LAST_SEEN_AT,
            D.LAST_SEEN_IP,
            D.ACTION_AT,
            AU.USERNAME AS ACTION_BY_NAME
        FROM M_USER_DEVICE D
        INNER JOIN M_USER U ON U.USER_SRNO = D.USER_SRNO
        LEFT JOIN M_USER AU ON AU.USER_SRNO = D.ACTION_BY
        WHERE D.DEL_FLAG = 0
    ) X
    WHERE (@DEVICE_STATUS IS NULL OR X.DEVICE_STATUS = @DEVICE_STATUS)
    ORDER BY CASE WHEN X.DEVICE_STATUS = 'PENDING' THEN 0 ELSE 1 END, X.REQUESTED_AT DESC;
END
GO

-- ---------------------------------------------------------------------
-- 6) Admin action on a device.
--    @ACTION: APPROVE (pending -> approved, 30 days)
--             RENEW   (approved or expired -> approved, 30 more days from now)
--             REJECT  (pending -> rejected)
--             REVOKE  (approved/expired -> revoked)
--    RESULT_CODE: 1 = done, 0 = not allowed in current state, -1 = not admin
-- ---------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[IU_DEVICE_ACTION]
    @DEVICE_SRNO INT,
    @ACTION      VARCHAR(10),
    @USER_SRNO   INT          -- the admin performing the action (from the JWT)
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.FN_IS_ADMIN_USER(@USER_SRNO) = 0
    BEGIN
        SELECT -1 AS RESULT_CODE, 'Only admin users can manage devices.' AS RESULT_MSG;
        RETURN;
    END

    DECLARE @NOW DATETIME = GETDATE();
    DECLARE @STATUS VARCHAR(20), @EXPIRES DATETIME, @OWNER INT;

    SELECT @STATUS = DEVICE_STATUS, @EXPIRES = EXPIRES_AT, @OWNER = USER_SRNO
    FROM M_USER_DEVICE
    WHERE DEVICE_SRNO = @DEVICE_SRNO AND DEL_FLAG = 0;

    IF @STATUS IS NULL
    BEGIN
        SELECT 0 AS RESULT_CODE, 'Device not found.' AS RESULT_MSG;
        RETURN;
    END

    SET @ACTION = UPPER(LTRIM(RTRIM(@ACTION)));

    IF @ACTION IN ('APPROVE', 'RENEW')
    BEGIN
        IF NOT (   (@ACTION = 'APPROVE' AND @STATUS = 'PENDING')
                OR (@ACTION = 'RENEW'   AND @STATUS = 'APPROVED'))
        BEGIN
            SELECT 0 AS RESULT_CODE,
                   CASE WHEN @ACTION = 'APPROVE' THEN 'Only pending requests can be approved.'
                        ELSE 'Only approved or expired devices can be renewed.' END AS RESULT_MSG;
            RETURN;
        END

        -- keep the 2-device limit when approving (another device may have been approved meanwhile)
        IF @ACTION = 'APPROVE'
           AND (SELECT COUNT(*) FROM M_USER_DEVICE
                WHERE USER_SRNO = @OWNER AND DEL_FLAG = 0 AND DEVICE_SRNO <> @DEVICE_SRNO
                  AND DEVICE_STATUS = 'APPROVED' AND EXPIRES_AT > @NOW) >= 2
        BEGIN
            SELECT 0 AS RESULT_CODE, 'This user already has 2 approved devices. Revoke one first.' AS RESULT_MSG;
            RETURN;
        END

        UPDATE M_USER_DEVICE
        SET DEVICE_STATUS = 'APPROVED',
            EXPIRES_AT    = DATEADD(DAY, 30, @NOW),
            ACTION_BY     = @USER_SRNO,
            ACTION_AT     = @NOW,
            MODIFIED_BY   = @USER_SRNO,
            MODIFIED_AT   = @NOW
        WHERE DEVICE_SRNO = @DEVICE_SRNO;

        SELECT 1 AS RESULT_CODE, 'Device approved until ' + CONVERT(VARCHAR(11), DATEADD(DAY, 30, @NOW), 106) + '.' AS RESULT_MSG;
        RETURN;
    END

    IF @ACTION = 'REJECT'
    BEGIN
        IF @STATUS <> 'PENDING'
        BEGIN
            SELECT 0 AS RESULT_CODE, 'Only pending requests can be rejected.' AS RESULT_MSG;
            RETURN;
        END

        UPDATE M_USER_DEVICE
        SET DEVICE_STATUS = 'REJECTED', ACTION_BY = @USER_SRNO, ACTION_AT = @NOW,
            MODIFIED_BY = @USER_SRNO, MODIFIED_AT = @NOW
        WHERE DEVICE_SRNO = @DEVICE_SRNO;

        SELECT 1 AS RESULT_CODE, 'Request rejected.' AS RESULT_MSG;
        RETURN;
    END

    IF @ACTION = 'REVOKE'
    BEGIN
        IF @STATUS <> 'APPROVED'   -- covers expired approvals too
        BEGIN
            SELECT 0 AS RESULT_CODE, 'Only approved devices can be revoked.' AS RESULT_MSG;
            RETURN;
        END

        UPDATE M_USER_DEVICE
        SET DEVICE_STATUS = 'REVOKED', ACTION_BY = @USER_SRNO, ACTION_AT = @NOW,
            MODIFIED_BY = @USER_SRNO, MODIFIED_AT = @NOW
        WHERE DEVICE_SRNO = @DEVICE_SRNO;

        SELECT 1 AS RESULT_CODE, 'Device revoked. It loses access within a minute.' AS RESULT_MSG;
        RETURN;
    END

    SELECT 0 AS RESULT_CODE, 'Unknown action.' AS RESULT_MSG;
END
GO
