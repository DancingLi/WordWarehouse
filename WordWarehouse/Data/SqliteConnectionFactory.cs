using Microsoft.Data.Sqlite;

namespace WordWarehouse.Data;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(DatabaseOptions options)
    {
        _connectionString = $"Data Source={options.DatabasePath}";
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
