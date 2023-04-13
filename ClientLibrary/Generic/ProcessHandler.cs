namespace ClientLibrary;

public abstract class ProcessHandler {}

public interface IFactory<out T>
{
  T Create();
}