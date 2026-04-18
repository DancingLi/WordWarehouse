using System.Collections.ObjectModel;
using WordWarehouse.Infrastructure;
using WordWarehouse.Models;
using WordWarehouse.Services;

namespace WordWarehouse.ViewModels;

public class ReviewViewModel : ObservableObject
{
    private readonly IReviewService _reviewService;
    private EntryViewModel? _selectedEntry;

    public ReviewViewModel(IReviewService reviewService)
    {
        _reviewService = reviewService;
        ReviewItems = new ObservableCollection<EntryViewModel>();
        MarkNewCommand = new RelayCommand(_ => UpdateSelectedStatus(LearningStatus.New), _ => SelectedEntry is not null);
        MarkLearningCommand = new RelayCommand(_ => UpdateSelectedStatus(LearningStatus.Learning), _ => SelectedEntry is not null);
        MarkMasteredCommand = new RelayCommand(_ => UpdateSelectedStatus(LearningStatus.Mastered), _ => SelectedEntry is not null);
        RefreshCommand = new RelayCommand(Refresh);
    }

    public ObservableCollection<EntryViewModel> ReviewItems { get; }

    public RelayCommand MarkNewCommand { get; }

    public RelayCommand MarkLearningCommand { get; }

    public RelayCommand MarkMasteredCommand { get; }

    public RelayCommand RefreshCommand { get; }

    public EntryViewModel? SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            if (SetProperty(ref _selectedEntry, value))
            {
                MarkNewCommand.RaiseCanExecuteChanged();
                MarkLearningCommand.RaiseCanExecuteChanged();
                MarkMasteredCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public void Refresh()
    {
        ReviewItems.Clear();
        foreach (var item in _reviewService.GetReviewItems().Where(x => x.Status != LearningStatus.Mastered))
        {
            ReviewItems.Add(new EntryViewModel(item));
        }
    }

    private void UpdateSelectedStatus(LearningStatus status)
    {
        if (SelectedEntry is null)
        {
            return;
        }

        _reviewService.UpdateStatus(SelectedEntry.Id, status);
        Refresh();
    }
}
