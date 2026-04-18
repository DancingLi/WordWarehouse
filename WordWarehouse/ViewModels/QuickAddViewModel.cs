using System.Collections.ObjectModel;
using WordWarehouse.Infrastructure;
using WordWarehouse.Models;
using WordWarehouse.Services;

namespace WordWarehouse.ViewModels;

public class QuickAddViewModel : ObservableObject
{
    private readonly IEntryService _entryService;
    private LanguageType _selectedLanguage = LanguageType.English;
    private EntryType _selectedEntryType = EntryType.Word;
    private string _content = string.Empty;
    private string? _meaning;
    private string? _example;
    private string? _tags;
    private string? _notes;
    private string _lastSavedMessage = "Ready.";

    public QuickAddViewModel(IEntryService entryService)
    {
        _entryService = entryService;
        Languages = new ObservableCollection<LanguageType>(Enum.GetValues<LanguageType>());
        EntryTypes = new ObservableCollection<EntryType>(Enum.GetValues<EntryType>());
        SaveCommand = new RelayCommand(SaveEntry, CanSaveEntry);
        ClearCommand = new RelayCommand(Clear);
    }

    public event EventHandler? EntrySaved;

    public ObservableCollection<LanguageType> Languages { get; }

    public ObservableCollection<EntryType> EntryTypes { get; }

    public RelayCommand SaveCommand { get; }

    public RelayCommand ClearCommand { get; }

    public LanguageType SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }

    public EntryType SelectedEntryType
    {
        get => _selectedEntryType;
        set => SetProperty(ref _selectedEntryType, value);
    }

    public string Content
    {
        get => _content;
        set
        {
            if (SetProperty(ref _content, value))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string? Meaning
    {
        get => _meaning;
        set => SetProperty(ref _meaning, value);
    }

    public string? Example
    {
        get => _example;
        set => SetProperty(ref _example, value);
    }

    public string? Tags
    {
        get => _tags;
        set => SetProperty(ref _tags, value);
    }

    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string LastSavedMessage
    {
        get => _lastSavedMessage;
        set => SetProperty(ref _lastSavedMessage, value);
    }

    private bool CanSaveEntry()
    {
        return !string.IsNullOrWhiteSpace(Content);
    }

    private void SaveEntry()
    {
        var entry = new Entry
        {
            Language = SelectedLanguage,
            EntryType = SelectedEntryType,
            Content = Content.Trim(),
            Meaning = Normalize(Meaning),
            Example = Normalize(Example),
            Tags = Normalize(Tags),
            Notes = Normalize(Notes),
            Status = LearningStatus.New
        };

        _entryService.CreateEntry(entry);
        Clear();
        EntrySaved?.Invoke(this, EventArgs.Empty);
        LastSavedMessage = $"Saved {SelectedEntryType.ToString().ToLowerInvariant()} to {SelectedLanguage}.";
    }

    private void Clear()
    {
        Content = string.Empty;
        Meaning = null;
        Example = null;
        Tags = null;
        Notes = null;
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
