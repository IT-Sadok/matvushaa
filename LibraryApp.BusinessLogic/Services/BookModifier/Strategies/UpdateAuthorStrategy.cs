using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;

namespace LibraryApp.BusinessLogic.Services.BookModifier.Strategies;

public class UpdateAuthorStrategy : IBookUpdateStrategy
{
    public BookInfoModel Modify(BookInfoModel book, int taskId)
    {
        book.Author = $"{book.Author} (Task: {taskId})";

        return book;
    }
}