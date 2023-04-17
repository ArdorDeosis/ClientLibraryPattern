namespace ModernTest;

public class Test
{
  private readonly string id;
  public Test(string id)
  {
    this.id = id;
  }

  public async Task DoStuff(object lockObject)
  {
    Console.WriteLine($"Started {id}");
    lock (lockObject)
    {
      while (true)
      {
        Monitor.Wait(lockObject);
        Console.WriteLine($"Update {id}");
      }
    }
  }
}