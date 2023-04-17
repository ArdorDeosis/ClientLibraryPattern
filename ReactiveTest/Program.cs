// See https://aka.ms/new-console-template for more information

using System.Reactive.Threading.Tasks;

// DistinctUntilChangedFilterTest.Run();


async Task<int> DoStuff()
{
  await Task.Delay(TimeSpan.FromSeconds(1));
  return 7;
}

DoStuff().ToObservable().Subscribe(
  onNext: x => Console.WriteLine($"next: {x}"),
  onCompleted: () => Console.WriteLine("completed"));

await Task.Delay(TimeSpan.FromSeconds(2)); 