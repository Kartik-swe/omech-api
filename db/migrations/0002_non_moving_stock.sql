/* =====================================================================
   0002 - Non-moving stock report (read-only)

   DT_NON_MOVING_STOCK lists stock that has not moved for @DAYS days or more:
     - Mother coils   (RAW_MATERIALS,      status 4,5   - same as Raw Material Stock)
     - Slit coils     (SLITTING_PROCESSES, status 4,5,8 - same as Raw Material Stock)
     - Pipes          (PIPE_INVENTORY,     qty > 0      - same as Pipe Inventory)

   "Last movement" = the latest of:
     coil : received date, any status change (T_LOG_MATERIAL_STATUS),
            a location shift (MATERIAL_SHIFTING_HISTORY), a slit cut from it
     slit : slitting date, status change, location shift, a re-slit cut from it
     pipe : any pipe transaction at that location (PIPE_INVENTORY_LOG,
            in or out), or the last time the stock row itself changed

   Only SELECTs - no table is created or changed. Safe to run again
   (CREATE OR ALTER).

   Result sets:
     Table  : one row per item, oldest first
     Table1 : total stock (all ages) per type, to show the non-moving share
   ===================================================================== */
CREATE OR ALTER PROCEDURE dbo.DT_NON_MOVING_STOCK
    @DAYS        INT     = 60,   -- not moved for at least this many days
    @ITEM_TYPE   CHAR(1) = 'A',  -- A = all, M = mother coils, S = slit coils, P = pipes
    @GRADE_SRNO  INT     = NULL,
    @C_LOCATION  INT     = NULL,
    @USER_SRNO   INT     = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @DAYS IS NULL OR @DAYS < 0 SET @DAYS = 60;
    IF @ITEM_TYPE IS NULL OR @ITEM_TYPE NOT IN ('A','M','S','P') SET @ITEM_TYPE = 'A';

    DECLARE @TODAY DATE = CAST(GETDATE() AS DATE);

    DECLARE @R TABLE (
        ITEM_TYPE       CHAR(1)        NOT NULL,   -- M / S / P
        ITEM_TYPE_NAME  VARCHAR(30)    NOT NULL,
        MATERIAL_SRNO   INT            NULL,
        SLITTING_SRNO   INT            NULL,
        PR_INV_SRNO     NUMERIC(18,0)  NULL,
        CHALLAN_NO      VARCHAR(100)   NULL,
        ITEM_NAME       NVARCHAR(150)  NULL,       -- size / pipe name
        GRADE           NVARCHAR(50)   NULL,
        THICKNESS       DECIMAL(18,2)  NULL,
        WIDTH           DECIMAL(18,2)  NULL,
        OD              NVARCHAR(50)   NULL,
        PR_LENGTH       NVARCHAR(50)   NULL,
        QUANTITY        NUMERIC(18,0)  NULL,       -- pipes: pcs, coils: 1
        WEIGHT_KG       DECIMAL(18,2)  NULL,
        RATE_PER_KG     DECIMAL(10,2)  NULL,
        STATUS_NAME     VARCHAR(50)    NULL,
        C_LOCATION_SRNO INT            NULL,
        C_LOCATION      VARCHAR(255)   NULL,
        IN_DATE         DATETIME       NULL,       -- received / slit / first stocked
        LAST_MOVE_DATE  DATETIME       NULL,
        LAST_MOVE_TYPE  VARCHAR(30)    NULL
    );

    /* ---------- Mother coils ---------- */
    IF @ITEM_TYPE IN ('A','M')
    INSERT INTO @R
    SELECT 'M', 'Mother Coil', RM.MATERIAL_SRNO, NULL, NULL, RM.CHALLAN_NO,
           CONCAT(CAST(MT.THICKNESS AS VARCHAR(20)), ' x ',
                  CAST(RM.MATERIAL_WIDTH - ISNULL(FN.SLITTED_WIDTH, 0) - ISNULL(RM.MATERIAL_SCRAP, 0) AS VARCHAR(20))),
           MG.GRADE, MT.THICKNESS,
           RM.MATERIAL_WIDTH - ISNULL(FN.SLITTED_WIDTH, 0) - ISNULL(RM.MATERIAL_SCRAP, 0),
           NULL, NULL, 1,
           RM.MATERIAL_WEIGHT - ISNULL(FN.SLITTED_WEIGHT, 0) - ISNULL(RM.MATERIAL_SCRAP_WEIGHT, 0),
           RM.RATE_PER_KG, MS.STATUS_NAME, RM.C_LOCATION, MV.VENDOR_NAME,
           ISNULL(RM.RECEIVED_DATE, RM.CREATED_AT),
           LM.LAST_MOVE_DATE, LM.LAST_MOVE_TYPE
    FROM RAW_MATERIALS RM WITH (NOLOCK)
    INNER JOIN M_STATUS    MS ON RM.MATERIAL_STATUS_SRNO    = MS.STATUS_SRNO
    INNER JOIN M_VENDORS   MV ON RM.C_LOCATION              = MV.VENDOR_SRNO
    INNER JOIN M_GRADE     MG ON RM.MATERIAL_GRADE_SRNO     = MG.GRADE_SRNO
    INNER JOIN M_THICKNESS MT ON RM.MATERIAL_THICKNESS_SRNO = MT.THICKNESS_SRNO
    CROSS APPLY dbo.FN_GET_SLITTED_DATA(1, RM.MATERIAL_SRNO) AS FN
    CROSS APPLY (
        SELECT TOP 1 X.D AS LAST_MOVE_DATE, X.T AS LAST_MOVE_TYPE
        FROM (
            SELECT ISNULL(RM.RECEIVED_DATE, RM.CREATED_AT), 'Received'
            UNION ALL
            SELECT MAX(L.STATUS_CHANGE_DATE), 'Status change'
            FROM T_LOG_MATERIAL_STATUS L WITH (NOLOCK)
            WHERE L.MATERIAL_SRNO = RM.MATERIAL_SRNO AND ISNULL(L.SLITTING_SRNO, 0) = 0
              AND ISNULL(L.DEL_FLAG, 0) = 0
            UNION ALL
            SELECT MAX(H.SHIFT_DATE), 'Location shift'
            FROM MATERIAL_SHIFTING_HISTORY H WITH (NOLOCK)
            WHERE H.MATERIAL_SRNO = RM.MATERIAL_SRNO AND ISNULL(H.SLITTING_SRNO, 0) = 0
              AND ISNULL(H.DEL_FLAG, 0) = 0
            UNION ALL
            SELECT MAX(C.SLITTING_DATE), 'Slitting'
            FROM SLITTING_PROCESSES C WITH (NOLOCK)
            WHERE C.MATERIAL_SRNO = RM.MATERIAL_SRNO AND C.SLITTING_LEVEL = 1
              AND ISNULL(C.DEL_FLAG, 0) = 0
        ) X (D, T)
        WHERE X.D IS NOT NULL
        ORDER BY X.D DESC
    ) LM
    WHERE RM.DEL_FLAG = 0
      AND RM.MATERIAL_STATUS_SRNO IN (4,5)
      AND RM.MATERIAL_WEIGHT - ISNULL(FN.SLITTED_WEIGHT, 0) - ISNULL(RM.MATERIAL_SCRAP_WEIGHT, 0) > 0
      AND (@GRADE_SRNO IS NULL OR RM.MATERIAL_GRADE_SRNO = @GRADE_SRNO)
      AND (@C_LOCATION IS NULL OR RM.C_LOCATION = @C_LOCATION);

    /* ---------- Slit coils (slitted + semi-slitted) ---------- */
    IF @ITEM_TYPE IN ('A','S')
    INSERT INTO @R
    SELECT 'S', CASE WHEN SP.IS_SLITTED = 1 THEN 'Slitted Coil' ELSE 'Semi Slitted Coil' END,
           SP.MATERIAL_SRNO, SP.SLITTING_SRNO, NULL, RM.CHALLAN_NO,
           CONCAT(CAST(MT.THICKNESS AS VARCHAR(20)), ' x ',
                  CAST(SP.SLITTING_WIDTH - ISNULL(FN.SLITTED_WIDTH, 0) - ISNULL(SP.SLITTING_SCRAP, 0) AS VARCHAR(20))),
           MG.GRADE, MT.THICKNESS,
           SP.SLITTING_WIDTH - ISNULL(FN.SLITTED_WIDTH, 0) - ISNULL(SP.SLITTING_SCRAP, 0),
           NULL, NULL, 1,
           SP.SLITTING_WEIGHT - ISNULL(FN.SLITTED_WEIGHT, 0) - ISNULL(SP.SLITTING_SCRAP_WEIGHT, 0),
           RM.RATE_PER_KG, MS.STATUS_NAME, SP.C_LOCATION, MV.VENDOR_NAME,
           ISNULL(SP.SLITTING_DATE, SP.CREATED_AT),
           LM.LAST_MOVE_DATE, LM.LAST_MOVE_TYPE
    FROM SLITTING_PROCESSES SP WITH (NOLOCK)
    INNER JOIN RAW_MATERIALS RM ON SP.MATERIAL_SRNO           = RM.MATERIAL_SRNO
    INNER JOIN M_STATUS      MS ON SP.STATUS_SRNO             = MS.STATUS_SRNO
    INNER JOIN M_VENDORS     MV ON SP.C_LOCATION              = MV.VENDOR_SRNO
    INNER JOIN M_GRADE       MG ON RM.MATERIAL_GRADE_SRNO     = MG.GRADE_SRNO
    INNER JOIN M_THICKNESS   MT ON RM.MATERIAL_THICKNESS_SRNO = MT.THICKNESS_SRNO
    CROSS APPLY dbo.FN_GET_SLITTED_DATA(0, SP.SLITTING_SRNO) AS FN
    CROSS APPLY (
        SELECT TOP 1 X.D AS LAST_MOVE_DATE, X.T AS LAST_MOVE_TYPE
        FROM (
            SELECT ISNULL(SP.SLITTING_DATE, SP.CREATED_AT), 'Slitted'
            UNION ALL
            SELECT MAX(L.STATUS_CHANGE_DATE), 'Status change'
            FROM T_LOG_MATERIAL_STATUS L WITH (NOLOCK)
            WHERE L.SLITTING_SRNO = SP.SLITTING_SRNO AND ISNULL(L.DEL_FLAG, 0) = 0
            UNION ALL
            SELECT MAX(H.SHIFT_DATE), 'Location shift'
            FROM MATERIAL_SHIFTING_HISTORY H WITH (NOLOCK)
            WHERE H.SLITTING_SRNO = SP.SLITTING_SRNO AND ISNULL(H.DEL_FLAG, 0) = 0
            UNION ALL
            SELECT MAX(C.SLITTING_DATE), 'Re-slitting'
            FROM SLITTING_PROCESSES C WITH (NOLOCK)
            WHERE C.SLITTING_SRNO_FK = SP.SLITTING_SRNO AND ISNULL(C.DEL_FLAG, 0) = 0
        ) X (D, T)
        WHERE X.D IS NOT NULL
        ORDER BY X.D DESC
    ) LM
    WHERE SP.DEL_FLAG = 0
      AND RM.DEL_FLAG = 0
      AND SP.STATUS_SRNO IN (4,5,8)
      AND SP.SLITTING_WEIGHT - ISNULL(FN.SLITTED_WEIGHT, 0) - ISNULL(SP.SLITTING_SCRAP_WEIGHT, 0) > 0
      AND (@GRADE_SRNO IS NULL OR RM.MATERIAL_GRADE_SRNO = @GRADE_SRNO)
      AND (@C_LOCATION IS NULL OR SP.C_LOCATION = @C_LOCATION);

    /* ---------- Pipes ---------- */
    IF @ITEM_TYPE IN ('A','P')
    INSERT INTO @R
    SELECT 'P', 'Pipe', NULL, NULL, P.PR_INV_SRNO, NULL,
           PM.PR_NAME, MG.GRADE, MT.THICKNESS, NULL, CAST(MO.OD AS NVARCHAR(50)), PM.PR_LENGTH,
           P.AVAILABLE_QUANTITY,
           PM.PR_WEIGHT * P.AVAILABLE_QUANTITY,
           NULL, NULL, CAST(P.C_LOCATION AS INT), MV.VENDOR_NAME,
           FI.FIRST_DATE,
           LM.LAST_MOVE_DATE, LM.LAST_MOVE_TYPE
    FROM PIPE_INVENTORY P WITH (NOLOCK)
    INNER JOIN PR_MASTER   PM ON PM.PR_SRNO        = P.PR_SRNO
    INNER JOIN M_GRADE     MG ON PM.GRADE_SRNO     = MG.GRADE_SRNO
    INNER JOIN M_THICKNESS MT ON PM.THICKNESS_SRNO = MT.THICKNESS_SRNO
    INNER JOIN M_OD        MO ON PM.OD_SRNO        = MO.OD_SRNO
    INNER JOIN M_VENDORS   MV ON MV.VENDOR_SRNO    = P.C_LOCATION
    OUTER APPLY (
        SELECT ISNULL(MIN(G.TRN_DATE), P.CREATED_AT) AS FIRST_DATE
        FROM PIPE_INVENTORY_LOG G WITH (NOLOCK)
        WHERE G.PR_SRNO = P.PR_SRNO AND G.C_LOCATION = P.C_LOCATION AND ISNULL(G.DEL_FLAG, 0) = 0
    ) FI
    CROSS APPLY (
        SELECT TOP 1 X.D AS LAST_MOVE_DATE, X.T AS LAST_MOVE_TYPE
        FROM (
            SELECT MAX(G.TRN_DATE), 'Pipe transaction'
            FROM PIPE_INVENTORY_LOG G WITH (NOLOCK)
            WHERE G.PR_SRNO = P.PR_SRNO
              AND (G.C_LOCATION = P.C_LOCATION OR G.FROM_LOCATION = P.C_LOCATION)
              AND ISNULL(G.DEL_FLAG, 0) = 0
            UNION ALL
            SELECT ISNULL(P.MODIFIED_AT, P.CREATED_AT), 'Stock updated'
        ) X (D, T)
        WHERE X.D IS NOT NULL
        ORDER BY X.D DESC
    ) LM
    WHERE P.DEL_FLAG = 0
      AND P.AVAILABLE_QUANTITY > 0
      AND (@GRADE_SRNO IS NULL OR PM.GRADE_SRNO = @GRADE_SRNO)
      AND (@C_LOCATION IS NULL OR P.C_LOCATION = @C_LOCATION);

    /* ---------- Table: non-moving items, oldest first ---------- */
    SELECT ITEM_TYPE, ITEM_TYPE_NAME, MATERIAL_SRNO, SLITTING_SRNO, PR_INV_SRNO,
           CHALLAN_NO, ITEM_NAME, GRADE, THICKNESS, WIDTH, OD, PR_LENGTH,
           QUANTITY, WEIGHT_KG, RATE_PER_KG,
           CAST(WEIGHT_KG * RATE_PER_KG AS DECIMAL(18,2)) AS STOCK_VALUE,
           STATUS_NAME, C_LOCATION_SRNO, C_LOCATION,
           CONVERT(VARCHAR(10), IN_DATE, 120)        AS IN_DATE,
           CONVERT(VARCHAR(10), LAST_MOVE_DATE, 120) AS LAST_MOVE_DATE,
           LAST_MOVE_TYPE,
           DATEDIFF(DAY, CAST(LAST_MOVE_DATE AS DATE), @TODAY) AS DAYS_IDLE
    FROM @R
    WHERE LAST_MOVE_DATE IS NULL
       OR DATEDIFF(DAY, CAST(LAST_MOVE_DATE AS DATE), @TODAY) >= @DAYS
    ORDER BY CASE WHEN LAST_MOVE_DATE IS NULL THEN 0 ELSE 1 END, LAST_MOVE_DATE, ITEM_TYPE;

    /* ---------- Table1: total stock per type (all ages) ---------- */
    SELECT ITEM_TYPE,
           COUNT(*)                     AS TOTAL_ITEMS,
           SUM(ISNULL(QUANTITY, 0))     AS TOTAL_QUANTITY,
           SUM(ISNULL(WEIGHT_KG, 0))    AS TOTAL_WEIGHT_KG
    FROM @R
    GROUP BY ITEM_TYPE;
END;
GO
