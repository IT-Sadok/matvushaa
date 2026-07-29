using CommonData.Enums;
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
            string status = book.Status.ToString();
            Console.WriteLine($"Selected book has status: {status}");
            
            try
            {
                BookStatus statusToSet = book.Status == BookStatus.Available
                    ? BookStatus.InUse
                    : BookStatus.Available;
                libraryService.UpdateBookStatus(bookId, statusToSet);
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