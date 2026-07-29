using System.Text.Json;
using LibraryApp.DataAccess.Interfaces;

namespace LibraryApp.DataAccess.Services;

public class FileStorageService<T> : IFileStorageService<T> where T : class
{
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.json");

    public List<T> LoadFromFile()
    {
        if (!File.Exists(_filePath)) return new List<T>();

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public void WriteToFile(List<T> data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);

        File.WriteAllText(_filePath, json);
    }
}