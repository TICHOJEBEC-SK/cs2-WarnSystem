using Dapper;

namespace WarnSystem.Data;

public class WarnRepository
{
    private readonly Database _db;

    public WarnRepository(Database db)
    {
        _db = db;
    }

    public async Task<WarnRecord?> GetAsync(ulong targetSteamId)
    {
        await using var con = _db.Open();
        const string q = @"
SELECT
  id,
  target_steamid    AS TargetSteamId,
  target_username   AS TargetUsername,
  total_warns       AS TotalWarns,
  active_warns      AS ActiveWarns,
  total_penalties   AS TotalPenalties,
  last_warn         AS LastWarn
FROM warn_system_warns
WHERE target_steamid = @targetSteamId;";
        return await con.QueryFirstOrDefaultAsync<WarnRecord>(q, new { targetSteamId });
    }

    public async Task UpsertAsync(WarnRecord rec)
    {
        await using var con = _db.Open();
        const string q = @"
INSERT INTO warn_system_warns
(target_steamid, target_username, total_warns, active_warns, total_penalties, last_warn)
VALUES
(@TargetSteamId, @TargetUsername, @TotalWarns, @ActiveWarns, @TotalPenalties, @LastWarn)
ON DUPLICATE KEY UPDATE
  target_username = VALUES(target_username),
  total_warns     = VALUES(total_warns),
  active_warns    = VALUES(active_warns),
  total_penalties = VALUES(total_penalties),
  last_warn       = VALUES(last_warn);";
        await con.ExecuteAsync(q, rec);
    }

    public async Task InsertLogAsync(WarnLogRecord log)
    {
        await using var con = _db.Open();
        const string q = @"
INSERT INTO warn_system_logs
(admin_steamid, admin_username, target_steamid, target_username, date_warn)
VALUES
(@AdminSteamId, @AdminUsername, @TargetSteamId, @TargetUsername, @DateWarn);";
        await con.ExecuteAsync(q, log);
    }
}