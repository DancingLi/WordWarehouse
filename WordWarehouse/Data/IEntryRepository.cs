using WordWarehouse.Models;

namespace WordWarehouse.Data;

public interface IEntryRepository
{
    Entry Create(Entry entry);

    void Update(Entry entry);

    void Delete(long id);

    Entry? GetById(long id);

    IReadOnlyList<Entry> Search(EntryFilter filter);

    IReadOnlyList<Entry> GetRecent(int count);

    int CountAll();

    int CountByStatus(LearningStatus status);
}
