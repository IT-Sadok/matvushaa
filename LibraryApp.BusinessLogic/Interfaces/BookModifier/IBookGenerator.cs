using CommonData.Models.Read;

namespace LibraryApp.BusinessLogic.Interfaces.BookModifier;

public interface IBookGenerator
{
    public BookInfoModel GenerateRandom(Random rand);
}