using Microsoft.Data.Sqlite;
using WordWarehouse.Models;

namespace WordWarehouse.Data;

public class SqliteEntryRepository : IEntryRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteEntryRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Entry Create(Entry entry)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Entries (
                Language, EntryType, Content, Meaning, Example, Notes, Tags,
                Status, CreatedAt, UpdatedAt, LastReviewedAt
            )
            VALUES (
                $language, $entryType, $content, $meaning, $example, $notes, $tags,
                $status, $createdAt, $updatedAt, $lastReviewedAt
            );
            SELECT last_insert_rowid();
            """;
        AddParameters(command, entry);
        entry.Id = (long)(command.ExecuteScalar() ?? 0L);
        return entry;
    }

    public void Update(Entry entry)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Entries
            SET Language = $language,
                EntryType = $entryType,
                Content = $content,
                Meaning = $meaning,
                Example = $example,
                Notes = $notes,
                Tags = $tags,
                Status = $status,
                CreatedAt = $createdAt,
                UpdatedAt = $updatedAt,
                LastReviewedAt = $lastReviewedAt
            WHERE Id = $id;
            """;
        AddParameters(command, entry);
        command.Parameters.AddWithValue("$id", entry.Id);
        command.ExecuteNonQuery();
    }

    public void Delete(long id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Entries WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    public Entry? GetById(long id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Entries WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapEntry(reader) : null;
    }

    public IReadOnlyList<Entry> Search(EntryFilter filter)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        var conditions = new List<string>();

        if (filter.Language.HasValue)
        {
            conditions.Add("Language = $language");
            command.Parameters.AddWithValue("$language", (int)filter.Language.Value);
        }

        if (filter.EntryType.HasValue)
        {
            conditions.Add("EntryType = $entryType");
            command.Parameters.AddWithValue("$entryType", (int)filter.EntryType.Value);
        }

        if (filter.Status.HasValue)
        {
            conditions.Add("Status = $status");
            command.Parameters.AddWithValue("$status", (int)filter.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Tag))
        {
            conditions.Add("IFNULL(Tags, '') LIKE $tag");
            command.Parameters.AddWithValue("$tag", $"%{filter.Tag.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            conditions.Add("(Content LIKE $search OR IFNULL(Meaning, '') LIKE $search)");
            command.Parameters.AddWithValue("$search", $"%{filter.SearchText.Trim()}%");
        }

        var whereClause = conditions.Count == 0 ? string.Empty : $"WHERE {string.Join(" AND ", conditions)}";
        command.CommandText = $"SELECT * FROM Entries {whereClause} ORDER BY UpdatedAt DESC, Id DESC;";

        var items = new List<Entry>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(MapEntry(reader));
        }

        return items;
    }

    public IReadOnlyList<Entry> GetRecent(int count)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Entries ORDER BY UpdatedAt DESC, Id DESC LIMIT $count;";
        command.Parameters.AddWithValue("$count", count);

        var items = new List<Entry>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(MapEntry(reader));
        }

        return items;
    }

    public int CountAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Entries;";
        return Convert.ToInt32(command.ExecuteScalar() ?? 0);
    }

    public int CountByStatus(LearningStatus status)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Entries WHERE Status = $status;";
        command.Parameters.AddWithValue("$status", (int)status);
        return Convert.ToInt32(command.ExecuteScalar() ?? 0);
    }

    private static void AddParameters(SqliteCommand command, Entry entry)
    {
        command.Parameters.AddWithValue("$language", (int)entry.Language);
        command.Parameters.AddWithValue("$entryType", (int)entry.EntryType);
        command.Parameters.AddWithValue("$content", entry.Content);
        command.Parameters.AddWithValue("$meaning", (object?)entry.Meaning ?? DBNull.Value);
        command.Parameters.AddWithValue("$example", (object?)entry.Example ?? DBNull.Value);
        command.Parameters.AddWithValue("$notes", (object?)entry.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("$tags", (object?)entry.Tags ?? DBNull.Value);
        command.Parameters.AddWithValue("$status", (int)entry.Status);
        command.Parameters.AddWithValue("$createdAt", entry.CreatedAt.ToString("O"));
        command.Parameters.AddWithValue("$updatedAt", entry.UpdatedAt.ToString("O"));
        command.Parameters.AddWithValue("$lastReviewedAt", entry.LastReviewedAt?.ToString("O") ?? (object)DBNull.Value);
    }

    private static Entry MapEntry(SqliteDataReader reader)
    {
        return new Entry
        {
            Id = reader.GetInt64(reader.GetOrdinal("Id")),
            Language = (LanguageType)reader.GetInt32(reader.GetOrdinal("Language")),
            EntryType = (EntryType)reader.GetInt32(reader.GetOrdinal("EntryType")),
            Content = reader.GetString(reader.GetOrdinal("Content")),
            Meaning = GetNullableString(reader, "Meaning"),
            Example = GetNullableString(reader, "Example"),
            Notes = GetNullableString(reader, "Notes"),
            Tags = GetNullableString(reader, "Tags"),
            Status = (LearningStatus)reader.GetInt32(reader.GetOrdinal("Status")),
            CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt"))),
            UpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("UpdatedAt"))),
            LastReviewedAt = GetNullableDateTime(reader, "LastReviewedAt")
        };
    }

    private static string? GetNullableString(SqliteDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static DateTime? GetNullableDateTime(SqliteDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : DateTime.Parse(reader.GetString(ordinal));
    }
}
