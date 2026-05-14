using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class GetBookByNameStepHandler(
    IInputService inputService,
    ILibraryService libraryService) : IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.GetBookByBookNameNumber;

    public string MenuStepName => "Get book by name";

    public bool Execute()
    {
        string bookName = inputService.ReadStringInput("Enter book name");
        try
        {
            var books = libraryService.GetByBookName(bookName);
            if (books.Count != 0)
            {
                foreach (var book in books)
                {
                    Console.WriteLine("{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}", "Id", "Name", "Author", "Year", "Status");
                    Console.WriteLine(
                        "{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}",
                        $"{book.Id}", $"{book.BookName}", $"{book.Author}", $"{book.ReleaseYear}", book.Status ? "Available" : "Not Available");
                }
            }
            else
            {
                Console.WriteLine($"There is no such book in library by this name: {bookName}");
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