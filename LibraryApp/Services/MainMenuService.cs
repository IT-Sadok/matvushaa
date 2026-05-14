using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services;

public class MainMenuService(
    IInputService inputService,
    IEnumerable<IMenuStepHandler> stepHandlers)
{
    public void Run()
    {
        bool isRun = true;
        while (isRun)
        {
            Console.Clear();
            Console.WriteLine("=====MY_BOOKS=====");
            foreach (IMenuStepHandler stepHandler in stepHandlers)
            {
                Console.WriteLine($"{stepHandler.MenuStepNumber}. {stepHandler.MenuStepName}");
            }
            Console.WriteLine("==================");
            
            int choice = inputService.ReadIntInput("Select menu number");
            
            var selectedStepHandler = stepHandlers.FirstOrDefault(s=>s.MenuStepNumber == choice);
            if (selectedStepHandler != null)
            {
                isRun = selectedStepHandler.Execute();
            }
            else
            {
                Console.WriteLine("Don`t be stupid pick from selected");
            }
            Console.ReadKey();
        }
    }
}