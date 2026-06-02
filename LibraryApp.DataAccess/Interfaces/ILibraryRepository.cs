using CommonData.Enums;
using CommonData.Models.Read;

namespace LibraryApp.DataAccess.Interfaces;

public interface ILibraryRepository
{
    public IReadOnlyCollection<BookInfoModel> GetAll();
    
    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action);
    
    public void Create(BookInfoModel createModel);

    public void UpdateBookStatus(int bookId, BookStatus status);
    
    public void UpdateBook(int bookId, BookInfoModel bookInfoModel);
    
    public void Delete(int bookId);

    public void SaveData();
}