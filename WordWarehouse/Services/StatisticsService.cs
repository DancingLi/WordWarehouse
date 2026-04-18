using WordWarehouse.Data;
using WordWarehouse.Models;

namespace WordWarehouse.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IEntryRepository _repository;

    public StatisticsService(IEntryRepository repository)
    {
        _repository = repository;
    }

    public DashboardStats GetDashboardStats()
    {
        return new DashboardStats
        {
            TotalCount = _repository.CountAll(),
            NewCount = _repository.CountByStatus(LearningStatus.New),
            LearningCount = _repository.CountByStatus(LearningStatus.Learning),
            MasteredCount = _repository.CountByStatus(LearningStatus.Mastered)
        };
    }

    public IReadOnlyList<Entry> GetRecentItems(int count)
    {
        return _repository.GetRecent(count);
    }
}
