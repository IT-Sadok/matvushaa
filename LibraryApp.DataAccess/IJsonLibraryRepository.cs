using CommonData.Models.Read;

namespace LibraryApp.DataAccess;

public interface IJsonLibraryRepository
{
    public IReadOnlyCollection<BookInfoModel> GetAll();
    
    public IReadOnlyCollection<BookInfoModel> GetByPredicate(Predicate<BookInfoModel> action);
    
    public void AddAndSave(BookInfoModel createModel);
    
    public void UpdateAndSave(int bookId, bool status);
    
    public void DeleteAndSave(int bookId);
}