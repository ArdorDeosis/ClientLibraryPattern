namespace ClientLibrary;

public abstract class ProcessHandler {}

public interface IFactory<T>
{
  ValueTask<T> Create();
}

internal class SimpleAsyncFactory<T> : IFactory<T>
{
  private readonly Func<ValueTask<T>> factoryMethod;

  internal SimpleAsyncFactory(Func<ValueTask<T>> factoryMethod)
  {
    this.factoryMethod = factoryMethod;
  }

  public ValueTask<T> Create() => factoryMethod();
}