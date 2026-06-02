using CommonData.Models.Read;

namespace LibraryApp.BusinessLogic.Interfaces.BookModifier;

public interface IBookUpdateStrategy
{
    BookInfoModel Modify(BookInfoModel book, int taskId);
}