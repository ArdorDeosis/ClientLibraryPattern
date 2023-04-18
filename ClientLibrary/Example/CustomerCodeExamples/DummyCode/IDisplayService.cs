namespace ClientLibrary;

public interface IDisplayService
{
	public void Display(string? message);
	public void Display(string message, ButtonDefinition button);
}

public sealed record ButtonDefinition(string Title, Action Action);