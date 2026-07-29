using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class DeleteBookStepHandler(
    IInputService inputService,
    ILibraryService libraryService): IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.DeleteBookNumber;
    public string MenuStepName => "Delete book";
    
    public bool Execute()
    {
        Console.WriteLine("Follow instructions:");
        int bookId = inputService.ReadIntInput("Enter bookId");

        try
        {
            libraryService.DeleteBook(bookId);
            Console.WriteLine("You successfully deleted book");
        }
        catch (Exception e)
        {
            Console.WriteLine("Sorry...");
            Console.WriteLine("We can not delete book with such id, check id and try again");
        }
        
        return true;
    }
}