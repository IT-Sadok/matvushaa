using CommonData.Enums;
using CommonData.Models.Read;

namespace LibraryApp.DataAccess.Interfaces;

public interface ILibraryRepository
{
    public IReadOnlyCollection<BookInfoModel> GetAll();
    
    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action);
    
    public void Create(BookInfoModel createModel);
    
    public void Update(int bookId, BookStatus status);
    
    public void Delete(int bookId);

    public void SaveData();
}