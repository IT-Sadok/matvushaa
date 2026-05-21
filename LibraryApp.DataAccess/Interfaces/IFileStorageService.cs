using CommonData.Models.Read;

namespace LibraryApp.DataAccess.Interfaces;

public interface IFileStorageService<T> where T : class
{
    public List<T> LoadFromFile();

    public void WriteToFile(List<T> data);
}