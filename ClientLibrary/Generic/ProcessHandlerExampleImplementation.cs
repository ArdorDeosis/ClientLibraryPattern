using System.Reactive.Linq;

namespace ClientLibrary;

public class ProcessHandlerExampleImplementation : ProcessHandlerExample
{
	public ProcessHandlerExampleImplementation()
	{
		var subscription = EventStream.DistinctUntilChanged().Subscribe(ReceivedData);
	}
	
	protected override async Task<Decision> MakeDecision(Data data, CancellationToken cancellationToken = default)
	{
		await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
		return SomeState.Number > 0 
			? Decision.Approve 
			: Decision.Deny;
	}
	
	// protected override async Task<Decision> MakeOptionalDecision(Data data, CancellationToken cancellationToken = default)
	// {
	// 	await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
	// 	return SomeState.Number > 0 
	// 		? Decision.Approve 
	// 		: Decision.Deny;
	// }

	private void ReceivedData(Data data)
	{
		if (data.Number > 1000)
			InterruptProcess().ContinueWith(task =>
			{
				if (task.Result.IsFailure)
				{
					// error handling
				}
			});
	}
}