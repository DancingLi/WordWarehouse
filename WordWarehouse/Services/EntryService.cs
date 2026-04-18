using WordWarehouse.Data;
using WordWarehouse.Models;

namespace WordWarehouse.Services;

public class EntryService : IEntryService
{
    private readonly IEntryRepository _repository;

    public EntryService(IEntryRepository repository)
    {
        _repository = repository;
    }

    public event EventHandler? EntriesChanged;

    public Entry CreateEntry(Entry entry)
    {
        var now = DateTime.UtcNow;
        entry.CreatedAt = now;
        entry.UpdatedAt = now;
        var created = _repository.Create(entry);
        EntriesChanged?.Invoke(this, EventArgs.Empty);
        return created;
    }

    public void UpdateEntry(Entry entry)
    {
        entry.UpdatedAt = DateTime.UtcNow;
        _repository.Update(entry);
        EntriesChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DeleteEntry(long id)
    {
        _repository.Delete(id);
        EntriesChanged?.Invoke(this, EventArgs.Empty);
    }

    public Entry? GetEntryById(long id)
    {
        return _repository.GetById(id);
    }

    public IReadOnlyList<Entry> SearchEntries(EntryFilter filter)
    {
        return _repository.Search(filter);
    }
}
