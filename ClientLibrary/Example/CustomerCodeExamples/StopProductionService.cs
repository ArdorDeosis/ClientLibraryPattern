namespace ClientLibrary;

// Scenario
// stop production on button click
// cancel production stop on another button click
public sealed class StopProductionService
{
	private readonly IPokeballCarvingStationProxy proxy;

	private WaitAndListenToCancelRequests_StopProductionProcessHandler? handler;

	public StopProductionService(IPokeballCarvingStationProxy proxy)
	{
		this.proxy = proxy;
	}

	private void OnButtonPressed()
	{
		handler = new WaitAndListenToCancelRequests_StopProductionProcessHandler();
		proxy.StopProduction(handler);
	}

	private async Task OnCancellationButtonPressed()
	{
		if (handler is null) return;
		var result = await handler.CancelProductionStop();
		if (result.IsFailure)
		{
			// error handling
		}
	}
}