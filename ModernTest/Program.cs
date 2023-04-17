// See https://aka.ms/new-console-template for more information


using ClientLibrary;

var eventStream = new ManualTerminatingAsyncEnumerable<int>();

eventStream.Push(1);
eventStream.Push(1);
eventStream.Push(1);

Task.Run(async () =>
{
  var rnd = new Random();
  while (!eventStream.HasTerminated)
  {
    await Task.Delay(1000);
    var n = rnd.Next(5);
    eventStream.Push(n);
    if (n is 0) eventStream.Terminate();
  }
});

Task.Run(async () =>
{
  await foreach (var number in eventStream) 
    Console.WriteLine($"from the off: {number}");
});

await foreach (var number in eventStream)
{
  Console.WriteLine(number);
}
await foreach (var number in eventStream.Where(n => n > 1))
{
  Console.WriteLine($"repeat {number}");
}