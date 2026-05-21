namespace LibraryApp.Interfaces;

public interface IMenuStepHandler
{
    public int MenuStepNumber { get; }

    public string MenuStepName { get; }

    public bool Execute();
}