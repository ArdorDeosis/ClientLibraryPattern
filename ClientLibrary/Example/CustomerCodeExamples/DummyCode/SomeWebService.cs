namespace ClientLibrary;

public static class SomeWebService
{
	public static Task<T> GetSome<T>(CancellationToken token) => Task.FromResult(default(T)!);
}