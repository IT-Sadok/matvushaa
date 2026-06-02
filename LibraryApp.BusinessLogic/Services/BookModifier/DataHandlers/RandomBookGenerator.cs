using CommonData.Enums;
using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;

namespace LibraryApp.BusinessLogic.Services.BookModifier.DataHandlers;

public class RandomBookGenerator : IBookGenerator
{
    public BookInfoModel GenerateRandom(Random rand)
    {
        return new BookInfoModel
        {
            Id = rand.Next(1, 999),
            BookName = $"Random Book: {rand.Next(1, 999)}",
            Author = $"Random Author: {rand.Next(1, 100)}",
            YearPublished = rand.Next(1900, 2026),
            Status = BookStatus.Available
        };
    }
}