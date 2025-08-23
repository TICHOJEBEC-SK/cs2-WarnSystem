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

        var sql = @"
CREATE TABLE IF NOT EXISTS warn_system (
  id INT NOT NULL AUTO_INCREMENT,
  steamid BIGINT UNSIGNED NOT NULL,
  username VARCHAR(64) NOT NULL,
  total_warns INT NOT NULL DEFAULT 0,
  active_warns INT NOT NULL DEFAULT 0,
  total_penalties INT NOT NULL DEFAULT 0,
  last_warn DATETIME NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY uq_warns_steamid (steamid)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
        await con.ExecuteAsync(sql);
    }
}