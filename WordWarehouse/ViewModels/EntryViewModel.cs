using WordWarehouse.Infrastructure;
using WordWarehouse.Models;

namespace WordWarehouse.ViewModels;

public class EntryViewModel : ObservableObject
{
    public EntryViewModel(Entry model)
    {
        Model = model;
    }

    public Entry Model { get; }

    public long Id => Model.Id;

    public string Language => Model.Language.ToString();

    public string EntryType => Model.EntryType.ToString();

    public string Content => Model.Content;

    public string Meaning => string.IsNullOrWhiteSpace(Model.Meaning) ? "-" : Model.Meaning!;

    public string Tags => string.IsNullOrWhiteSpace(Model.Tags) ? "-" : Model.Tags!;

    public string Status => Model.Status.ToString();

    public string UpdatedAt => Model.UpdatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
}
