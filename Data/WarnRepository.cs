using Dapper;

namespace WarnSystem.Data;

public class WarnRepository
{
    private readonly Database _db;

    public WarnRepository(Database db)
    {
        _db = db;
    }

    public async Task<WarnRecord?> GetAsync(ulong steamId)
    {
        await using var con = _db.Open();
        const string q =
            "SELECT id, steamid, username, total_warns AS TotalWarns, active_warns AS ActiveWarns, total_penalties AS TotalPenalties, last_warn AS LastWarn FROM warn_system WHERE steamid=@steamId;";
        return await con.QueryFirstOrDefaultAsync<WarnRecord>(q, new { steamId });
    }

    public async Task UpsertAsync(WarnRecord rec)
    {
        await using var con = _db.Open();
        const string q = @"
INSERT INTO warn_system (steamid, username, total_warns, active_warns, total_penalties, last_warn)
VALUES (@SteamId, @Username, @TotalWarns, @ActiveWarns, @TotalPenalties, @LastWarn)
ON DUPLICATE KEY UPDATE
  username=VALUES(username),
  total_warns=VALUES(total_warns),
  active_warns=VALUES(active_warns),
  total_penalties=VALUES(total_penalties),
  last_warn=VALUES(last_warn);";
        await con.ExecuteAsync(q, rec);
    }
}