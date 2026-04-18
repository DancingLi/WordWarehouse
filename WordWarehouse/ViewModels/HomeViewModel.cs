using System.Collections.ObjectModel;
using WordWarehouse.Infrastructure;
using WordWarehouse.Services;

namespace WordWarehouse.ViewModels;

public class HomeViewModel : ObservableObject
{
    private readonly IStatisticsService _statisticsService;
    private int _totalCount;
    private int _newCount;
    private int _learningCount;
    private int _masteredCount;

    public HomeViewModel(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
        RecentEntries = new ObservableCollection<EntryViewModel>();
    }

    public int TotalCount
    {
        get => _totalCount;
        set => SetProperty(ref _totalCount, value);
    }

    public int NewCount
    {
        get => _newCount;
        set => SetProperty(ref _newCount, value);
    }

    public int LearningCount
    {
        get => _learningCount;
        set => SetProperty(ref _learningCount, value);
    }

    public int MasteredCount
    {
        get => _masteredCount;
        set => SetProperty(ref _masteredCount, value);
    }

    public ObservableCollection<EntryViewModel> RecentEntries { get; }

    public void Refresh()
    {
        var stats = _statisticsService.GetDashboardStats();
        TotalCount = stats.TotalCount;
        NewCount = stats.NewCount;
        LearningCount = stats.LearningCount;
        MasteredCount = stats.MasteredCount;

        RecentEntries.Clear();
        foreach (var item in _statisticsService.GetRecentItems(8))
        {
            RecentEntries.Add(new EntryViewModel(item));
        }
    }
}
