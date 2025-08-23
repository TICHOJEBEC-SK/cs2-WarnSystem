using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace WarnSystem.Services;

public static class Chat
{
    private static readonly Dictionary<string, char> ColorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["default"] = ChatColors.Default,
        ["white"] = ChatColors.White,
        ["darkred"] = ChatColors.DarkRed,
        ["green"] = ChatColors.Green,
        ["lightyellow"] = ChatColors.LightYellow,
        ["lightblue"] = ChatColors.LightBlue,
        ["olive"] = ChatColors.Olive,
        ["lime"] = ChatColors.Lime,
        ["red"] = ChatColors.Red,
        ["lightpurple"] = ChatColors.LightPurple,
        ["purple"] = ChatColors.Purple,
        ["grey"] = ChatColors.Grey,
        ["gray"] = ChatColors.Grey,
        ["yellow"] = ChatColors.Yellow,
        ["gold"] = ChatColors.Gold,
        ["silver"] = ChatColors.Silver,
        ["blue"] = ChatColors.Blue,
        ["darkblue"] = ChatColors.DarkBlue,
        ["bluegrey"] = ChatColors.BlueGrey,
        ["magenta"] = ChatColors.Magenta,
        ["lightred"] = ChatColors.LightRed,
        ["orange"] = ChatColors.Orange
    };


    private static string ApplyColors(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;


        var t = text;
        foreach (var kv in ColorMap)
            t = t.Replace("{" + kv.Key + "}", kv.Value.ToString());
        return t;
    }


    private static void ToAll(string message, bool ensureReset = true)
    {
        var msg = ApplyColors(message);
        if (ensureReset) msg += ChatColors.Default;
        Server.PrintToChatAll(msg);
    }


    public static void ToAllFmt(string fmt, params object[] args)
    {
        var templ = ApplyColors(fmt);
        var msg = args.Length == 0 ? templ : string.Format(templ, args);
        ToAll(msg);
    }


    public static void ToPlayer(CCSPlayerController player, string fmt, params object[] args)
    {
        var templ = ApplyColors(fmt);
        var msg = args.Length == 0 ? templ : string.Format(templ, args);
        msg += ChatColors.Default;
        player.PrintToChat(msg);
    }


    public static bool ValidateCaller(CCSPlayerController? caller)
    {
        return caller is { IsValid: true } && !caller.IsBot && !caller.IsHLTV;
    }


    public static string Name(CCSPlayerController player)
    {
        return string.IsNullOrWhiteSpace(player.PlayerName) ? "Unknown" : player.PlayerName;
    }
}