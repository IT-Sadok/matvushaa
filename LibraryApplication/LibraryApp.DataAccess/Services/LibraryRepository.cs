using CommonData.Enums;
using CommonData.Models.Read;
using LibraryApp.DataAccess.Interfaces;

namespace LibraryApp.DataAccess.Services;

public class LibraryRepository : ILibraryRepository
{
    private readonly List<BookInfoModel> _data;
    private readonly IFileStorageService<BookInfoModel> _fileStorageService;

    public LibraryRepository(IFileStorageService<BookInfoModel> fileStorageService)
    {
        _fileStorageService = fileStorageService;
        _data = _fileStorageService.LoadFromFile();
    }

    public IReadOnlyCollection<BookInfoModel> GetAll() =>_data;

    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action) => _data.FindAll(action);

    public void Create(BookInfoModel createModel)
    {
        _data.Add(createModel);
    }

    public void Update(int bookId, BookStatus status)
    {
        var books = GetByPredicate(d => d.Id == bookId);
        if (books.Count != 0)
        {
            books.FirstOrDefault().Status = status;
        }
    }

    public void Delete(int bookId)
    {
        var books = GetByPredicate(d => d.Id == bookId);
        if (books.Count != 0)
        {
            var book = books.FirstOrDefault();
            _data.Remove(book);
        }
    }

    public void SaveData() => _fileStorageService.WriteToFile(_data);
}