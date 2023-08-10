using System.Reactive.Linq;

namespace ClientLibrary;

public sealed class ProcessHandlerExampleImplementation : ProcessHandlerExample, IDisposable
{
	private readonly ICollection<IDisposable> disposables = new List<IDisposable>();

	public ProcessHandlerExampleImplementation()
	{
		disposables.Add(DataStream
			.DistinctUntilChanged()
			.Subscribe(ReceivedData));
	}
	
	protected override async Task<Decision> MakeDecision(Data data, CancellationToken cancellationToken = default)
	{
		await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
		return Random.Shared.Next(2) == 0 
			? Decision.Approve 
			: Decision.Deny;
	}
	
	private void ReceivedData(Data data)
	{
		if (data.Number > 9000)
			Console.WriteLine("Number was over 9000!");
	}

	public void Dispose()
	{
		foreach (var disposable in disposables) 
			disposable.Dispose();
	}
}