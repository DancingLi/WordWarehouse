using System.Windows;
using WordWarehouse.Models;
using WordWarehouse.Services;
using WordWarehouse.ViewModels;

namespace WordWarehouse.Views;

public partial class EntryEditorWindow : Window
{
    private readonly IEntryService _entryService;
    private readonly Entry _entry;

    public EntryEditorWindow(Entry entry, IEntryService entryService)
    {
        _entry = entry;
        _entryService = entryService;
        InitializeComponent();
        DataContext = new EntryEditorViewModel(entry);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not EntryEditorViewModel viewModel)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(viewModel.Content))
        {
            MessageBox.Show("Content is required.", "WordWarehouse", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _entryService.UpdateEntry(viewModel.ToEntry(_entry));
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
