namespace WordWarehouse.Models;

public class EntryFilter
{
    public LanguageType? Language { get; set; }

    public EntryType? EntryType { get; set; }

    public LearningStatus? Status { get; set; }

    public string? Tag { get; set; }

    public string? SearchText { get; set; }
}
