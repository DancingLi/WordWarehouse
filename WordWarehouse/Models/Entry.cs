namespace WordWarehouse.Models;

public class Entry
{
    public long Id { get; set; }

    public LanguageType Language { get; set; }

    public EntryType EntryType { get; set; }

    public string Content { get; set; } = string.Empty;

    public string? Meaning { get; set; }

    public string? Example { get; set; }

    public string? Notes { get; set; }

    public string? Tags { get; set; }

    public LearningStatus Status { get; set; } = LearningStatus.New;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastReviewedAt { get; set; }
}
