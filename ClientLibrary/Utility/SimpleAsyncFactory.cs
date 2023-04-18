namespace ClientLibrary;

internal class SimpleAsyncFactory<T> : IFactory<T>
{
	private readonly Func<Task<T>> factoryMethod;

	internal SimpleAsyncFactory(Func<Task<T>> factoryMethod)
	{
		this.factoryMethod = factoryMethod;
	}

	public Task<T> Create() => factoryMethod();
}