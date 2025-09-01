using CounterStrikeSharp.API.Core;
using WarnSystem.Config;
using WarnSystem.Data;

namespace WarnSystem.Services;

public record WarnResult(int ActiveWarns, int TotalWarns, bool Penalty, string? PenaltyCommand);

public class WarnService
{
    private readonly WarnRepository _repo;
    private readonly WarnConfig _cfg;

    public WarnService(WarnRepository repo, WarnConfig cfg)
    {
        _repo = repo;
        _cfg = cfg;
    }
    
    public async Task<WarnResult> WarnAsync(CCSPlayerController admin, CCSPlayerController target)
    {
        var adminSteamId64 = admin.SteamID;
        var adminName = string.IsNullOrWhiteSpace(admin.PlayerName) ? "Unknown" : admin.PlayerName;

        var steamId64 = target.SteamID;
        var userId = target.UserId;
        var username = string.IsNullOrWhiteSpace(target.PlayerName) ? "Unknown" : target.PlayerName;
        
        var rec = await _repo.GetAsync(steamId64) ?? new WarnRecord
        {
            TargetSteamId = steamId64,
            TargetUsername = username,
            TotalWarns = 0,
            ActiveWarns = 0,
            TotalPenalties = 0,
            LastWarn = DateTime.Now
        };
        
        rec.TargetUsername = username;
        rec.TotalWarns += 1;
        rec.ActiveWarns += 1;
        rec.LastWarn = DateTime.Now;
        
        var log = new WarnLogRecord
        {
            AdminSteamId = adminSteamId64,
            AdminUsername = adminName,
            TargetSteamId = steamId64,
            TargetUsername = username,
            DateWarn = DateTime.Now
        };
        await _repo.InsertLogAsync(log);

        var triggered = rec.ActiveWarns >= _cfg.WarnThreshold;
        string? penaltyCmd = null;

        if (triggered)
        {
            var nextTotalPenalties = rec.TotalPenalties + 1;
            var baseMinutes = Math.Max(1, _cfg.PenaltyBaseMinutes);

            int durationMinutes = _cfg.PenaltyScalingEnabled
                ? baseMinutes * nextTotalPenalties
                : baseMinutes;

            var penaltyCmdTemplate = _cfg.PenaltyCommand ?? string.Empty;

            penaltyCmd = penaltyCmdTemplate
                .Replace("{steamid64}", steamId64.ToString())
                .Replace("{userid}", userId.ToString())
                .Replace("{username}", username)
                .Replace("{warns}", rec.TotalWarns.ToString())
                .Replace("{totalpenalties}", nextTotalPenalties.ToString())
                .Replace("{minutes}", durationMinutes.ToString());

            rec.TotalPenalties = nextTotalPenalties;
            rec.ActiveWarns = _cfg.ResetActiveWarnsAfterPenalty ? 0 : _cfg.WarnThreshold;
        }

        await _repo.UpsertAsync(rec);

        return new WarnResult(rec.ActiveWarns, rec.TotalWarns, triggered, penaltyCmd);
    }
}
