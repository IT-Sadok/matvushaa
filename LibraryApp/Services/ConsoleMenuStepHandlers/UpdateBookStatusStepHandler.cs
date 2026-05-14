using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class UpdateBookStatusStepHandler(
    IInputService inputService,
    ILibraryService libraryService): IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.UpdateBookStatusNumber;
    
    public string MenuStepName => "Update book status";
    
    public bool Execute()
    {
        Console.WriteLine("Follow instructions:");

        int bookId = inputService.ReadIntInput("Enter bookId to update status");
        var book = libraryService.GetById(bookId);

        if (book?.Id != null)
        {
            string status = book.Status
                ? "Available"
                : "Not available";
            Console.WriteLine($"Selected book has status: {status}");
            
            try
            {
                libraryService.UpdateBookStatus(bookId, !book.Status);
                Console.WriteLine("Success! Status was updated");
            }
            catch (Exception e)
            {
                Console.WriteLine("Oops...");
                Console.WriteLine("we got a problem, try again");
            }
        }
        
        return true;
    }
}