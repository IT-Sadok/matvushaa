using LibraryApp.Constants;
using LibraryApp.Interfaces;

namespace LibraryApp.Services;

public class ConsoleInputService : IInputService
{
    public string ReadStringInput(string lineName)
    {
        while (true)
        {
            Console.WriteLine($"{lineName}: ");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }

            Console.WriteLine(DisplayValueConstants.InvalidValueMessage);
        }
    }
    
    public int ReadIntInput(string lineName)
    {
        while (true)
        {
            Console.WriteLine($"{lineName}: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input >= DisplayValueConstants.MinIntValue) return input;
            }

            Console.WriteLine(DisplayValueConstants.InvalidValueMessage);
        }
    }
}