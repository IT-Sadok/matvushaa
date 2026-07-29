namespace LibraryApp.Interfaces;

public interface IInputService
{
    string ReadStringInput(string lineName);

    int ReadIntInput(string lineName);
}