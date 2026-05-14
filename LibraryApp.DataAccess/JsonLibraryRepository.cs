using System.Text.Json;
using CommonData.Models.Read;

namespace LibraryApp.DataAccess;

public class JsonLibraryRepository : IJsonLibraryRepository
{
    private List<BookInfoModel> _data;
    private readonly string _filePath;

    public JsonLibraryRepository()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.json");
        _data = LoadFromFile();
    }

    private List<BookInfoModel> LoadFromFile()
    {
        if (!File.Exists(_filePath)) return new List<BookInfoModel>();

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<BookInfoModel>>(json) ?? new List<BookInfoModel>();
    }

    private void WriteToFile()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_data, options);
        File.WriteAllText(_filePath, json);
    }

    public IReadOnlyCollection<BookInfoModel> GetAll()
    {
        return _data = LoadFromFile();
    }

    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action)
    {
        GetAll();
        return _data.FindAll(action);
    }

    public void AddAndSave(BookInfoModel createModel)
    {
        _data.Add(createModel);
        WriteToFile();
    }

    public void UpdateAndSave(int bookId, bool status)
    {
        GetAll();
        var books = GetByPredicate(d => d.Id == bookId);
        if (books.Count != 0)
        {
            books.FirstOrDefault().Status = status;
        }

        WriteToFile();
    }

    public void DeleteAndSave(int bookId)
    {
        GetAll();
        var books = GetByPredicate(d => d.Id == bookId);
        if (books.Count != 0)
        {
            var book = books.FirstOrDefault();
            _data.Remove(book);
        }

        WriteToFile();
    }
}