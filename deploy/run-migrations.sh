#!/usr/bin/env bash
# Runs on the droplet during an API deploy (called by .github/workflows/deploy.yml).
#
#   run-migrations.sh pending <migrations-dir>          -> prints how many scripts have not run yet
#   run-migrations.sh apply   <migrations-dir> <sha>    -> runs them in order, each exactly once
#
# Scripts live in db/migrations/ in this repo, named NNNN_short_name.sql
# (e.g. 0002_add_xyz.sql). Applied scripts are recorded in DB_MIGRATION_LOG.
# Stops at the first failing script (sqlcmd -b); the deploy then stops too,
# BEFORE the new API code goes live.
#
# Rules for scripts: additive and re-runnable (IF NOT EXISTS / CREATE OR ALTER),
# never edited after they have been merged - add a new numbered script instead.
#
# DB access: same way as /omech/backup_sql.sh (mssql-tools container on the
# database container's network); the login is read from that script, so no
# database password is stored in GitHub.
set -euo pipefail

MODE="${1:?usage: run-migrations.sh pending|apply <dir> [sha]}"
DIR="$(cd "${2:?migrations dir required}" && pwd)"
SHA="${3:-}"
ROOT="${OMECH_ROOT:-/omech}"
DB_NAME="OMECH_PROD"
TOOLS_IMAGE="mcr.microsoft.com/mssql-tools"
NAME_RE='^[0-9]{4}_[A-Za-z0-9_]+\.sql$'

# --- login: reuse the credentials already on the droplet in backup_sql.sh ---
# read the values literally (no eval), with or without surrounding quotes
read_var() {
  sed -n -E "s/^$1=[\"']?([^\"']*)[\"']?[[:space:]]*\$/\1/p" "$ROOT/backup_sql.sh" | head -n 1
}
SA_USER="$(read_var SA_USER)"
SA_PASSWORD="$(read_var SA_PASSWORD)"
if [ -z "$SA_USER" ] || [ -z "$SA_PASSWORD" ]; then
  echo "ERROR: could not read SA_USER / SA_PASSWORD from $ROOT/backup_sql.sh" >&2
  exit 1
fi
export SQLCMDPASSWORD="$SA_PASSWORD"   # passed by name, so it never shows in 'ps'

sqlcmd() {
  docker run --rm -i --network container:database \
    -e SQLCMDPASSWORD -v "$DIR":/m:ro "$TOOLS_IMAGE" \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U "$SA_USER" -d "$DB_NAME" -b -I "$@"
}

ensure_log_table() {
  sqlcmd -Q "SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.DB_MIGRATION_LOG', N'U') IS NULL
CREATE TABLE dbo.DB_MIGRATION_LOG (
    MIGRATION_SRNO INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DB_MIGRATION_LOG PRIMARY KEY,
    FILE_NAME      VARCHAR(200) NOT NULL CONSTRAINT UX_DB_MIGRATION_LOG_FILE UNIQUE,
    CHECKSUM       CHAR(64)     NOT NULL,
    GIT_SHA        VARCHAR(40)  NULL,
    APPLIED_AT     DATETIME     NOT NULL CONSTRAINT DF_DB_MIGRATION_LOG_APPLIED_AT DEFAULT (GETDATE())
);" >/dev/null
}

applied_list() {
  sqlcmd -h -1 -W -Q "SET NOCOUNT ON; SELECT FILE_NAME + '|' + CHECKSUM FROM dbo.DB_MIGRATION_LOG" | sed '/^\s*$/d'
}

pending_files() {
  local applied f sum
  applied="$(applied_list)"
  for f in $(ls -1 "$DIR" | grep -E "$NAME_RE" | sort); do
    sum="$(sha256sum "$DIR/$f" | cut -c1-64)"
    if printf '%s\n' "$applied" | cut -d'|' -f1 | grep -qxF "$f"; then
      if ! printf '%s\n' "$applied" | grep -qxF "$f|$sum"; then
        echo "WARN: $f was changed after it was applied - not re-running it (add a new script instead)" >&2
      fi
    else
      echo "$f"
    fi
  done
}

# unknown files in the folder are a mistake, not something to silently skip
for f in $(ls -1 "$DIR"); do
  if ! printf '%s' "$f" | grep -Eq "$NAME_RE"; then
    echo "ERROR: unexpected file in migrations folder: $f (expected NNNN_name.sql)" >&2
    exit 1
  fi
done

ensure_log_table

case "$MODE" in
  pending)
    pending_files | wc -l | tr -d ' '
    ;;
  apply)
    files="$(pending_files)"
    if [ -z "$files" ]; then
      echo "DB scripts: nothing new to run"
      exit 0
    fi
    for f in $files; do
      sum="$(sha256sum "$DIR/$f" | cut -c1-64)"
      echo "DB script: running $f ..."
      if ! sqlcmd -i "/m/$f"; then
        echo "ERROR: DB script $f failed - deploy stopped, the running API was not changed" >&2
        exit 1
      fi
      sqlcmd -Q "SET NOCOUNT ON; INSERT INTO dbo.DB_MIGRATION_LOG (FILE_NAME, CHECKSUM, GIT_SHA) VALUES ('$f', '$sum', '${SHA//[^0-9a-f]/}')" >/dev/null
      echo "DB script: $f done"
    done
    ;;
  *)
    echo "unknown mode: $MODE" >&2
    exit 2
    ;;
esac
