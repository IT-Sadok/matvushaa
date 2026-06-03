using LibraryApp.BusinessLogic.Interfaces.BookModifier;
using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class TasksWorkStepHandler(
    IInputService inputService,
    IBookModifierService bookModifierService): IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.TasksWorkNumber;
    
    public string MenuStepName => "Random book organizer";
    
    public bool Execute()
    {
        Console.WriteLine("Work has begun. Please wait until it is finished.:");

        try
        {
            bookModifierService.RunModificationWorkAsync();
            Console.WriteLine("Success! The work is finished!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Oops...");
            Console.WriteLine("We got a problem, try again");
        }
            
        return true;
    }
}