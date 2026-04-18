namespace WordWarehouse.Data;

public class DatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Entries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Language INTEGER NOT NULL,
                EntryType INTEGER NOT NULL,
                Content TEXT NOT NULL,
                Meaning TEXT NULL,
                Example TEXT NULL,
                Notes TEXT NULL,
                Tags TEXT NULL,
                Status INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                LastReviewedAt TEXT NULL
            );

            CREATE INDEX IF NOT EXISTS IX_Entries_Language ON Entries(Language);
            CREATE INDEX IF NOT EXISTS IX_Entries_Status ON Entries(Status);
            CREATE INDEX IF NOT EXISTS IX_Entries_UpdatedAt ON Entries(UpdatedAt);
            """;
        command.ExecuteNonQuery();
    }
}
