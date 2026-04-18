using WordWarehouse.Models;

namespace WordWarehouse.Services;

public class ReviewService : IReviewService
{
    private readonly IEntryService _entryService;

    public ReviewService(IEntryService entryService)
    {
        _entryService = entryService;
    }

    public IReadOnlyList<Entry> GetReviewItems()
    {
        return _entryService.SearchEntries(new EntryFilter());
    }

    public void UpdateStatus(long id, LearningStatus status)
    {
        var entry = _entryService.GetEntryById(id);
        if (entry is null)
        {
            return;
        }

        entry.Status = status;
        entry.LastReviewedAt = DateTime.UtcNow;
        _entryService.UpdateEntry(entry);
    }

    public void MarkReviewed(long id)
    {
        var entry = _entryService.GetEntryById(id);
        if (entry is null)
        {
            return;
        }

        entry.LastReviewedAt = DateTime.UtcNow;
        _entryService.UpdateEntry(entry);
    }
}
