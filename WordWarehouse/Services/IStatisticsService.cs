using WordWarehouse.Models;

namespace WordWarehouse.Services;

public interface IStatisticsService
{
    DashboardStats GetDashboardStats();

    IReadOnlyList<Entry> GetRecentItems(int count);
}
