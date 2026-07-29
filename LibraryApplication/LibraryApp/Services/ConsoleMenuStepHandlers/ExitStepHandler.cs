using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services.ConsoleMenuStepHandlers;

public class ExitStepHandler : IMenuStepHandler
{
    public int MenuStepNumber => MenuItemsConstants.ExitMenuNumber;

    public string MenuStepName => "Exit";

    public bool Execute()
    {
        Console.WriteLine("Goodbye!");
        
        return false;
    }
}