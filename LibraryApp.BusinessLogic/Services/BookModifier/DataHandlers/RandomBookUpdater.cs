using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;

namespace LibraryApp.BusinessLogic.Services.BookModifier.DataHandlers;

public class RandomBookUpdater(IEnumerable<IBookUpdateStrategy> strategies) : IBookUpdater
{
    private readonly IBookUpdateStrategy[] _strategies = strategies.ToArray();

    public BookInfoModel UpdateRandomly(BookInfoModel book, Random rand, int taskId) 
        => _strategies[rand.Next(_strategies.Length)].Modify(book, taskId);
}