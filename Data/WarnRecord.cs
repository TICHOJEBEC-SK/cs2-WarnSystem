namespace WarnSystem.Data;

public class WarnRecord
{
    public int Id { get; set; }
    public ulong TargetSteamId { get; set; }
    public string TargetUsername { get; set; } = "";
    public int TotalWarns { get; set; }
    public int ActiveWarns { get; set; }
    public int TotalPenalties { get; set; }
    public DateTime LastWarn { get; set; }
}