using System.Collections.ObjectModel;
using WordWarehouse.Infrastructure;
using WordWarehouse.Models;

namespace WordWarehouse.ViewModels;

public class EntryEditorViewModel : ObservableObject
{
    private LanguageType _language;
    private EntryType _entryType;
    private string _content;
    private string? _meaning;
    private string? _example;
    private string? _notes;
    private string? _tags;
    private LearningStatus _status;

    public EntryEditorViewModel(Entry entry)
    {
        EntryId = entry.Id;
        _language = entry.Language;
        _entryType = entry.EntryType;
        _content = entry.Content;
        _meaning = entry.Meaning;
        _example = entry.Example;
        _notes = entry.Notes;
        _tags = entry.Tags;
        _status = entry.Status;

        Languages = new ObservableCollection<LanguageType>(Enum.GetValues<LanguageType>());
        EntryTypes = new ObservableCollection<EntryType>(Enum.GetValues<EntryType>());
        Statuses = new ObservableCollection<LearningStatus>(Enum.GetValues<LearningStatus>());
    }

    public long EntryId { get; }

    public ObservableCollection<LanguageType> Languages { get; }

    public ObservableCollection<EntryType> EntryTypes { get; }

    public ObservableCollection<LearningStatus> Statuses { get; }

    public LanguageType Language
    {
        get => _language;
        set => SetProperty(ref _language, value);
    }

    public EntryType EntryType
    {
        get => _entryType;
        set => SetProperty(ref _entryType, value);
    }

    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
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

    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string? Tags
    {
        get => _tags;
        set => SetProperty(ref _tags, value);
    }

    public LearningStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public Entry ToEntry(Entry original)
    {
        original.Language = Language;
        original.EntryType = EntryType;
        original.Content = Content.Trim();
        original.Meaning = Normalize(Meaning);
        original.Example = Normalize(Example);
        original.Notes = Normalize(Notes);
        original.Tags = Normalize(Tags);
        original.Status = Status;
        return original;
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
