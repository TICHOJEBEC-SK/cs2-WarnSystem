using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using MenuManager;
using WarnSystem.Config;
using WarnSystem.Data;
using WarnSystem.Menu;
using WarnSystem.Services;

namespace WarnSystem;

public class WarnSystem : BasePlugin, IPluginConfig<WarnConfig>
{
    public override string ModuleName => "WarnSystem";
    public override string ModuleVersion => "1.1";
    public override string ModuleAuthor => "TICHOJEBEC";
    public override string ModuleDescription => "https://github.com/TICHOJEBEC-SK/cs2-WarnSystem";

    private readonly PluginCapability<IMenuApi?> _menuCapability = new("menu:nfcore");
    private IMenuApi? _menuApi;

    private Localization _l = null!;
    private Database _db = null!;
    private WarnRepository _repo = null!;
    private WarnService _warns = null!;

    public WarnConfig Config { get; set; } = new();

    public void OnConfigParsed(WarnConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Language)) config.Language = "sk";
        if (string.IsNullOrWhiteSpace(config.ChatPrefix)) config.ChatPrefix = " [WARN]";
        if (string.IsNullOrWhiteSpace(config.WarnCommand)) config.WarnCommand = "warn";
        if (string.IsNullOrWhiteSpace(config.AdminPermission)) config.AdminPermission = "@css/ban";
        if (config.WarnThreshold <= 1) config.WarnThreshold = 3;

        Config = config;
    }

    public override void Load(bool hotReload)
    {
        var langDir = Path.Combine(ModuleDirectory, "lang");
        _l = new Localization(langDir, Config.Language);

        _db = new Database(Config.Database.ToConnectionString());
        _db.InitAsync().GetAwaiter().GetResult();
        _repo = new WarnRepository(_db);
        _warns = new WarnService(_repo, Config);

        AddCommand(Config.WarnCommand, "Open warn menu", OnWarnCommand);
    }

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        _menuApi = _menuCapability.Get();
        if (_menuApi == null)
            Console.WriteLine("[WarnSystem] MenuManager Core not found (menu:nfcore)");
    }

    private string Pref(string s)
    {
        return $"{Config.ChatPrefix} {s}";
    }

    private bool HasAdmin(CCSPlayerController p)
    {
        if (string.IsNullOrWhiteSpace(Config.AdminPermission))
            return true;

        return AdminManager.PlayerHasPermissions(p, Config.AdminPermission);
    }

    private void OnWarnCommand(CCSPlayerController? caller, CommandInfo info)
    {
        if (!Chat.ValidateCaller(caller)) return;
        var admin = caller!;

        if (!HasAdmin(admin))
        {
            Chat.ToPlayer(admin, Pref(_l["NoPermission"]), Config.WarnCommand);
            return;
        }

        if (_menuApi == null)
        {
            Chat.ToPlayer(admin, Pref(_l["MenuNotReady"]));
            return;
        }

        var players = Utilities.GetPlayers()
            .Where(p => p is { IsValid: true } && !p.IsBot && !p.IsHLTV)
            .OrderBy(p => p.PlayerName)
            .ToList();

        WarnMenu.Open(
            _menuApi,
            admin,
            players,
            _l["MenuTitle"],
            target =>
            {
                var result = _warns.WarnAsync(target).GetAwaiter().GetResult();

                if (!result.Penalty)
                {
                    Chat.ToAllFmt(Pref(_l["WarnBroadcast"]), Chat.Name(target), result.ActiveWarns);
                    Chat.ToPlayer(admin, Pref(_l["WarnFeedback"]), Chat.Name(target), result.ActiveWarns);
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.PenaltyCommand))
                        Server.NextFrame(() => Server.ExecuteCommand(result.PenaltyCommand!));
                }
            },
            _ => Chat.ToPlayer(admin, Pref(_l["MenuNoPlayers"]))
        );
    }
}