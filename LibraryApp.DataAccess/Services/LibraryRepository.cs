using CommonData.Enums;
using CommonData.Models.Read;
using LibraryApp.DataAccess.Interfaces;

namespace LibraryApp.DataAccess.Services;

public class LibraryRepository : ILibraryRepository
{
    private readonly List<BookInfoModel> _data;
    private readonly IFileStorageService<BookInfoModel> _fileStorageService;
    private readonly object _lock = new();
    private readonly SemaphoreSlim _semaphore = new(10);

    public LibraryRepository(IFileStorageService<BookInfoModel> fileStorageService)
    {
        _fileStorageService = fileStorageService;
        _data = _fileStorageService.LoadFromFile();
    }

    public IReadOnlyCollection<BookInfoModel> GetAll() =>
        ExecuteSafe(() => _data.ToList());

    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action) =>
        ExecuteSafe(() => _data.FindAll(action));

    public void Create(BookInfoModel createModel) =>
        ExecuteSafe(() => _data.Add(createModel));

    public void UpdateBookStatus(int bookId, BookStatus status) =>
        ExecuteSafe(() =>
        {
            {
                var books = GetByPredicate(d => d.Id == bookId);
                if (books.Count != 0)
                {
                    books.FirstOrDefault().Status = status;
                }
            }
        });

    public void UpdateBook(int bookId, BookInfoModel bookInfoModel) =>
        ExecuteSafe(() =>
        {
            {
                var books = GetByPredicate(d => d.Id == bookId);
                if (books.Count != 0)
                {
                    var bookToUpdate = books.FirstOrDefault();
                    bookToUpdate = bookInfoModel;
                }
            }
        });

    public void Delete(int bookId) =>
        ExecuteSafe(() => _data.RemoveAll(b => b.Id == bookId));

    public void SaveData() 
    {
        lock (_lock)
        {
            _fileStorageService.WriteToFile(_data);
        }
    }

    private T ExecuteSafe<T>(Func<T> operation)
    {
        _semaphore.Wait();
        try
        {
            lock (_lock)
            {
                return operation();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void ExecuteSafe(Action operation)
    {
        _semaphore.Wait();
        try
        {
            lock (_lock)
            {
                operation();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}