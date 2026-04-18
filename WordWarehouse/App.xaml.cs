using System.IO;
using System.Windows;
using WordWarehouse.Data;
using WordWarehouse.Services;
using WordWarehouse.ViewModels;

namespace WordWarehouse;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var appDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WordWarehouse");
        Directory.CreateDirectory(appDirectory);

        var databaseOptions = new DatabaseOptions
        {
            DatabasePath = Path.Combine(appDirectory, "wordwarehouse.db")
        };

        var connectionFactory = new SqliteConnectionFactory(databaseOptions);
        var initializer = new DatabaseInitializer(connectionFactory);
        initializer.Initialize();

        var repository = new SqliteEntryRepository(connectionFactory);
        var entryService = new EntryService(repository);
        var reviewService = new ReviewService(entryService);
        var statisticsService = new StatisticsService(repository);
        var mainViewModel = new MainViewModel(entryService, reviewService, statisticsService);

        var mainWindow = new MainWindow(mainViewModel, entryService, reviewService);
        mainWindow.Show();
    }
}
