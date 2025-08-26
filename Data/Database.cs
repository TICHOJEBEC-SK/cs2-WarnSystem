using Dapper;
using MySqlConnector;

namespace WarnSystem.Data;

public class Database
{
    private readonly string _connStr;

    public Database(string connStr)
    {
        _connStr = connStr;
    }

    public MySqlConnection Open()
    {
        return new MySqlConnection(_connStr);
    }

    public async Task InitAsync()
    {
        await using var con = Open();
        await con.OpenAsync();
        
        var sqlLog = @"
CREATE TABLE IF NOT EXISTS warn_system_logs (
  id INT NOT NULL AUTO_INCREMENT,
  admin_steamid   BIGINT UNSIGNED NOT NULL,
  admin_username  VARCHAR(64)     NOT NULL,
  target_steamid  BIGINT UNSIGNED NOT NULL,
  target_username VARCHAR(64)     NOT NULL,
  date_warn       DATETIME        NOT NULL,
  PRIMARY KEY (id),
  KEY idx_log_admin_steamid (admin_steamid),
  KEY idx_log_target_steamid (target_steamid),
  KEY idx_log_date_warn (date_warn)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
        await con.ExecuteAsync(sqlLog);
        
        var sqlWarns = @"
CREATE TABLE IF NOT EXISTS warn_system_warns (
  id INT NOT NULL AUTO_INCREMENT,
  target_steamid    BIGINT UNSIGNED NOT NULL,
  target_username   VARCHAR(64)     NOT NULL,
  total_warns       INT             NOT NULL DEFAULT 0,
  active_warns      INT             NOT NULL DEFAULT 0,
  total_penalties   INT             NOT NULL DEFAULT 0,
  last_warn         DATETIME        NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY uq_warns_target_steamid (target_steamid)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
        await con.ExecuteAsync(sqlWarns);
    }
}