using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace WarnSystem.Config;

public class DatabaseConfig
{
    [JsonPropertyName("Server")] public string Server { get; set; } = "127.0.0.1";
    [JsonPropertyName("Port")] public int Port { get; set; } = 3306;
    [JsonPropertyName("Database")] public string Database { get; set; } = "database_name";
    [JsonPropertyName("User")] public string User { get; set; } = "database_user";
    [JsonPropertyName("Password")] public string Password { get; set; } = "database_password";
    [JsonPropertyName("SslMode")] public string SslMode { get; set; } = "None";

    public string ToConnectionString()
    {
        return $"Server={Server};Port={Port};Database={Database};User ID={User};Password={Password};SslMode={SslMode};";
    }
}

public class WarnConfig : BasePluginConfig
{
    [JsonPropertyName("Language")] public string Language { get; set; } = "en";
    [JsonPropertyName("ChatPrefix")] public string ChatPrefix { get; set; } = " {lightred}[WARN]";
    [JsonPropertyName("WarnCommand")] public string WarnCommand { get; set; } = "warn";
    [JsonPropertyName("AdminPermission")] public string AdminPermission { get; set; } = "@css/ban";
    [JsonPropertyName("Database")] public DatabaseConfig Database { get; set; } = new();
    [JsonPropertyName("WarnThreshold")] public int WarnThreshold { get; set; } = 3;
    [JsonPropertyName("PenaltyScalingEnabled")] public bool PenaltyScalingEnabled { get; set; } = true;
    [JsonPropertyName("PenaltyBaseMinutes")] public int PenaltyBaseMinutes { get; set; } = 60;
    [JsonPropertyName("PenaltyCommand")] public string PenaltyCommand { get; set; } = "css_gag #{steamid64} {minutes} Toxic {username}";
    [JsonPropertyName("ResetActiveWarnsAfterPenalty")]
    public bool ResetActiveWarnsAfterPenalty { get; set; } = true;
}

