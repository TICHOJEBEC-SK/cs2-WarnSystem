using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using MenuManager;
using WarnSystem.Services;

namespace WarnSystem.Menu;

public static class WarnMenu
{
    public static void Open(
        IMenuApi api,
        CCSPlayerController admin,
        IEnumerable<CCSPlayerController> targets,
        string title,
        Action<CCSPlayerController> onSelect,
        Action<string>? onEmpty = null,
        Localization? loc = null)
    {
        var list = targets.ToList();
        if (list.Count == 0)
        {
            if (loc != null)
                Chat.ToPlayer(admin, $" {loc["MenuNoPlayers"]}");
            else
                onEmpty?.Invoke("MenuNoPlayers");
            return;
        }

        var menu = api.GetMenu(title);
        
        menu.PostSelectAction = PostSelectAction.Close;
        menu.ExitButton = true;

        foreach (var p in list)
        {
            var target = p;
            menu.AddMenuOption(Chat.Name(target), (ply, option) =>
            {
                if (target is { IsValid: true } && !target.IsBot && !target.IsHLTV)
                {
                    onSelect(target);
                    CounterStrikeSharp.API.Modules.Menu.MenuManager.CloseActiveMenu(admin);
                }
                else
                {
                    var offline = loc?["WarnTargetOffline"] ?? " {lightred}Player dont exist.";
                    Chat.ToPlayer(admin, offline);
                }
            });
        }

        menu.Open(admin);
    }
}