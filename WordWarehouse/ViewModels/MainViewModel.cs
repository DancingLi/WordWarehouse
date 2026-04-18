using WordWarehouse.Services;

namespace WordWarehouse.ViewModels;

public class MainViewModel
{
    public MainViewModel(IEntryService entryService, IReviewService reviewService, IStatisticsService statisticsService)
    {
        Home = new HomeViewModel(statisticsService);
        QuickAdd = new QuickAddViewModel(entryService);
        Library = new LibraryViewModel(entryService);
        Review = new ReviewViewModel(reviewService);

        entryService.EntriesChanged += (_, _) => RefreshAll();
        QuickAdd.EntrySaved += (_, _) => RefreshAll();

        RefreshAll();
    }

    public HomeViewModel Home { get; }

    public QuickAddViewModel QuickAdd { get; }

    public LibraryViewModel Library { get; }

    public ReviewViewModel Review { get; }

    public void RefreshAll()
    {
        Home.Refresh();
        Library.Refresh();
        Review.Refresh();
    }
}
