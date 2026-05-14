using CommonData.Models.Write;
using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class CreateBookStepHandler(
    IInputService inputService,
    ILibraryService libraryService): IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.AddNewBookNumber;
    
    public string MenuStepName => "Add new book";
    
    public bool Execute()
    {
        Console.WriteLine("Follow instructions:");
        var createBookModel = new BookCreateModel
        {
            BookName = inputService.ReadStringInput("Enter book name"),
            Author = inputService.ReadStringInput("Enter author"),
            ReleaseYear = inputService.ReadIntInput("Enter release year"),
        };

        try
        {
            libraryService.AddNewBook(createBookModel);
            Console.WriteLine("Success! New book added to library");
        }
        catch (Exception e)
        {
            Console.WriteLine("Oops...");
            Console.WriteLine("we got a problem with your book try again");
        }
            
        return true;
    }
}