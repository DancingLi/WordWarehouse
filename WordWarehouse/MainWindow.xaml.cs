using System.Windows;
using WordWarehouse.Services;
using WordWarehouse.ViewModels;
using WordWarehouse.Views;

namespace WordWarehouse;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel, IEntryService entryService, IReviewService reviewService)
    {
        InitializeComponent();
        DataContext = viewModel;

        HomeHost.Content = new HomeView { DataContext = viewModel.Home };
        QuickAddHost.Content = new QuickAddView { DataContext = viewModel.QuickAdd };
        LibraryHost.Content = new LibraryView(entryService, viewModel.Library) { DataContext = viewModel.Library };
        ReviewHost.Content = new ReviewView(entryService, reviewService, viewModel.Review) { DataContext = viewModel.Review };
    }
}
