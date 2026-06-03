using CommonData.Models.Read;

namespace LibraryApp.BusinessLogic.Interfaces.BookModifier;

public interface IBookUpdater
{
    public BookInfoModel UpdateRandomly(BookInfoModel book, Random rand, int taskId);
}