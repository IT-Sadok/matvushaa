using CommonData.Enums;
using CommonData.Models.Write;
using CommonData.Models.Read;

namespace LibraryApp.BusinessLogic.Interfaces;

public interface ILibraryService
{
    public IReadOnlyCollection<BookInfoModel> GetAllBooks();

    public BookInfoModel GetById(int bookId);
    
    public IReadOnlyCollection<BookInfoModel> GetByAuthorName(string authorName);
    
    public IReadOnlyCollection<BookInfoModel> GetByBookName(string authorName);
    
    public void AddNewBook(BookCreateModel bookCreateModel);

    public void UpdateBookStatus(int bookId, BookStatus statusToUpdate);

    public void DeleteBook(int bookId);
}