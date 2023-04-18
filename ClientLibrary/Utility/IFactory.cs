namespace ClientLibrary;

public interface IFactory<T>
{
	Task<T> Create();
}