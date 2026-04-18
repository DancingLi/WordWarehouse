using WordWarehouse.Models;

namespace WordWarehouse.Services;

public interface IEntryService
{
    event EventHandler? EntriesChanged;

    Entry CreateEntry(Entry entry);

    void UpdateEntry(Entry entry);

    void DeleteEntry(long id);

    Entry? GetEntryById(long id);

    IReadOnlyList<Entry> SearchEntries(EntryFilter filter);
}
