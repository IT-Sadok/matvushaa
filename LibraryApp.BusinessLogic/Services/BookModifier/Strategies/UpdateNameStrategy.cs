using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;

namespace LibraryApp.BusinessLogic.Services.BookModifier.Strategies;

public class UpdateNameStrategy : IBookUpdateStrategy
{
    public BookInfoModel Modify(BookInfoModel book, int taskId)
    {
        book.BookName = $"{book.BookName} (Task: {taskId})";

        return book;
    }
}