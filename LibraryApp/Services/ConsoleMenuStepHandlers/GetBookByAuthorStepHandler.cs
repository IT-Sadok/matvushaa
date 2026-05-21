using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class GetBookByAuthorStepHandler(
    IInputService inputService,
    ILibraryService libraryService) : IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.GetBookByAuthorNameNumber;

    public string MenuStepName => "Get book by author";

    public bool Execute()
    {
        string authorName = inputService.ReadStringInput("Enter author name");
        try
        {
            var books = libraryService.GetByAuthorName(authorName);
            if (books.Count != 0)
            {
                Console.WriteLine("{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}", "Id", "Name", "Author", "Year", "Status");
                foreach (var book in books)
                {
                    Console.WriteLine(
                        "{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}",
                        $"{book.Id}", $"{book.BookName}", $"{book.Author}", $"{book.YearPublished}", book.Status.ToString());
                }
            }
            else
            {
                Console.WriteLine($"There is no such book in library by this author: {authorName}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Oops...");
            Console.WriteLine("we got a problem with your book try again");
        }

        return true;
    }
}