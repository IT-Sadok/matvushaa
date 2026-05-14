using CommonData.Models.Read;
using CommonData.Models.Write;
using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.DataAccess;

namespace LibraryApp.BusinessLogic.Services;

public class LibraryService(IJsonLibraryRepository bookRepository) : ILibraryService
{
    public IReadOnlyCollection<BookInfoModel> GetAllBooks()
    {
        var result = bookRepository.GetAll();

        return result;
    }

    public BookInfoModel GetById(int bookId)
    {
        var books = bookRepository.GetByPredicate(d => d.Id == bookId);
        if (books.Count != 0)
        {
            return books.FirstOrDefault();
        }
        else
        {
            return null;
        }
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
            ReleaseYear = bookCreateModel.ReleaseYear,
            Status = true,
        };

        bookRepository.AddAndSave(addNew);
    }

    public void UpdateBookStatus(int bookId, bool statusToUpdate)
    {
        bookRepository.UpdateAndSave(bookId, statusToUpdate);
    }

    public void DeleteBook(int bookId)
    {
        bookRepository.DeleteAndSave(bookId);
    }
}