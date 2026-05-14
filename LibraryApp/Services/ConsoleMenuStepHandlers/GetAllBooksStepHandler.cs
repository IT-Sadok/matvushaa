using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;
using CommonData.Models.Read;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class GetAllBooksStepHandler(ILibraryService libraryService) : IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.GetAllBooksNumber;

    public string MenuStepName => "Retrieve all books";

    public bool Execute()
    {
        try
        {
            Console.WriteLine("Our Books:");
            var booksList = libraryService.GetAllBooks();
            Console.WriteLine("{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}", "Id", "Name", "Author", "Year", "Status");
            
            foreach (var book in booksList)
            {
                Console.WriteLine(
                    "{0,-2} | {1,-20} | {2,-20} | {3,-6} | {4,-10}",
                    $"{book.Id}", $"{book.BookName}", $"{book.Author}", $"{book.ReleaseYear}", book.Status ? "Available" : "Not Available");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("We facing some issues, try again");
        }
        
        return true;
    }
}