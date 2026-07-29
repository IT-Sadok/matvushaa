using CommonData.Enums;
using CommonData.Models.Read;
using CommonData.Models.Write;
using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.DataAccess.Interfaces;

namespace LibraryApp.BusinessLogic.Services;

public class LibraryService(ILibraryRepository bookRepository) : ILibraryService
{
    public IReadOnlyCollection<BookInfoModel> GetAllBooks()
    {
        var result = bookRepository.GetAll();

        return result;
    }

    public BookInfoModel GetById(int bookId)
    {
        var books = bookRepository.GetByPredicate(d => d.Id == bookId);

        return books.FirstOrDefault();
    }

    public IReadOnlyCollection<BookInfoModel> GetByAuthorName(string authorName)
    {
        return bookRepository.GetByPredicate(d => d.Author.Contains(authorName));
    }

    public IReadOnlyCollection<BookInfoModel> GetByBookName(string bookName)
    {
        return bookRepository.GetByPredicate(d => d.BookName.Contains(bookName));
    }

    public void AddNewBook(BookCreateModel bookCreateModel)
    {
        var addNew = new BookInfoModel
        {
            Id = GetAllBooks().Count,
            BookName = bookCreateModel.BookName,
            Author = bookCreateModel.Author,
            YearPublished = bookCreateModel.YearPublished,
            Status = BookStatus.Available,
        };

        bookRepository.Create(addNew);
        bookRepository.SaveData();
    }

    public void UpdateBookStatus(int bookId, BookStatus statusToUpdate)
    {
        bookRepository.Update(bookId, statusToUpdate);
        bookRepository.SaveData();
    }

    public void DeleteBook(int bookId)
    {
        bookRepository.Delete(bookId);
        bookRepository.SaveData();
    }
}