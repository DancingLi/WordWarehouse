using System.Collections.ObjectModel;
using WordWarehouse.Infrastructure;
using WordWarehouse.Models;
using WordWarehouse.Services;

namespace WordWarehouse.ViewModels;

public class LibraryViewModel : ObservableObject
{
    private readonly IEntryService _entryService;
    private LanguageType? _selectedLanguage;
    private EntryType? _selectedEntryType;
    private LearningStatus? _selectedStatus;
    private string? _tag;
    private string? _searchText;
    private EntryViewModel? _selectedEntry;

    public LibraryViewModel(IEntryService entryService)
    {
        _entryService = entryService;
        Entries = new ObservableCollection<EntryViewModel>();
        LanguageOptions = new ObservableCollection<EnumOption<LanguageType>>(BuildOptions<LanguageType>());
        EntryTypeOptions = new ObservableCollection<EnumOption<EntryType>>(BuildOptions<EntryType>());
        StatusOptions = new ObservableCollection<EnumOption<LearningStatus>>(BuildOptions<LearningStatus>());
        ApplyFiltersCommand = new RelayCommand(Refresh);
        ClearFiltersCommand = new RelayCommand(ClearFilters);
    }

    public ObservableCollection<EntryViewModel> Entries { get; }

    public ObservableCollection<EnumOption<LanguageType>> LanguageOptions { get; }

    public ObservableCollection<EnumOption<EntryType>> EntryTypeOptions { get; }

    public ObservableCollection<EnumOption<LearningStatus>> StatusOptions { get; }

    public RelayCommand ApplyFiltersCommand { get; }

    public RelayCommand ClearFiltersCommand { get; }

    public EntryViewModel? SelectedEntry
    {
        get => _selectedEntry;
        set => SetProperty(ref _selectedEntry, value);
    }

    public LanguageType? SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }

    public EntryType? SelectedEntryType
    {
        get => _selectedEntryType;
        set => SetProperty(ref _selectedEntryType, value);
    }

    public LearningStatus? SelectedStatus
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    public string? Tag
    {
        get => _tag;
        set => SetProperty(ref _tag, value);
    }

    public string? SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public void Refresh()
    {
        Entries.Clear();
        var filter = new EntryFilter
        {
            Language = SelectedLanguage,
            EntryType = SelectedEntryType,
            Status = SelectedStatus,
            Tag = Tag,
            SearchText = SearchText
        };

        foreach (var item in _entryService.SearchEntries(filter))
        {
            Entries.Add(new EntryViewModel(item));
        }
    }

    public void ClearFilters()
    {
        SelectedLanguage = null;
        SelectedEntryType = null;
        SelectedStatus = null;
        Tag = null;
        SearchText = null;
        Refresh();
    }

    private static IEnumerable<EnumOption<T>> BuildOptions<T>() where T : struct, Enum
    {
        yield return new EnumOption<T> { Label = "All", Value = null };

        foreach (var value in Enum.GetValues<T>())
        {
            yield return new EnumOption<T> { Label = value.ToString(), Value = value };
        }
    }
}
