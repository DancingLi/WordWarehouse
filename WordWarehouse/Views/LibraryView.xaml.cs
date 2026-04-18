using System.Windows;
using System.Windows.Controls;
using WordWarehouse.Services;
using WordWarehouse.ViewModels;

namespace WordWarehouse.Views;

public partial class LibraryView : UserControl
{
    private readonly IEntryService _entryService;
    private readonly LibraryViewModel _viewModel;

    public LibraryView(IEntryService entryService, LibraryViewModel viewModel)
    {
        _entryService = entryService;
        _viewModel = viewModel;
        InitializeComponent();
    }

    private void EditSelected_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedEntry is null)
        {
            MessageBox.Show("Select an entry first.", "WordWarehouse", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var entry = _entryService.GetEntryById(_viewModel.SelectedEntry.Id);
        if (entry is null)
        {
            return;
        }

        var window = new EntryEditorWindow(entry, _entryService)
        {
            Owner = Window.GetWindow(this)
        };
        window.ShowDialog();
    }

    private void DeleteSelected_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedEntry is null)
        {
            MessageBox.Show("Select an entry first.", "WordWarehouse", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Delete \"{_viewModel.SelectedEntry.Content}\"?",
            "WordWarehouse",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _entryService.DeleteEntry(_viewModel.SelectedEntry.Id);
        }
    }
}
