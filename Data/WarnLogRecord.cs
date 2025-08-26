namespace WarnSystem.Data;

public class WarnLogRecord
{
    public int Id { get; set; }
    public ulong AdminSteamId { get; set; }
    public string AdminUsername { get; set; } = "";
    public ulong TargetSteamId { get; set; }
    public string TargetUsername { get; set; } = "";
    public DateTime DateWarn { get; set; }
}