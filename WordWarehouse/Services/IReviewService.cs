using WordWarehouse.Models;

namespace WordWarehouse.Services;

public interface IReviewService
{
    IReadOnlyList<Entry> GetReviewItems();

    void UpdateStatus(long id, LearningStatus status);

    void MarkReviewed(long id);
}
