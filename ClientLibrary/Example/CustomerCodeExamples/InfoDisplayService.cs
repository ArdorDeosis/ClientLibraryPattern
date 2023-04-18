namespace ClientLibrary;

// Scenario:
// Display data from the server when a button is pressed. If the data is not available, display "N/A"
public sealed class InfoDisplayService
{
	private const string NoDataString = "N/A";
	
	private readonly IPokeballCarvingStationProxy proxy;
	private readonly IDisplayService displayService;

	public InfoDisplayService(IPokeballCarvingStationProxy proxy, IDisplayService displayService)
	{
		this.proxy = proxy;
		this.displayService = displayService;
	}

	private async Task OnButtonPressed()
	{
		var result = await proxy.GetMachineData();
		displayService.Display(result.IsSuccess ? result.Data.ToString() : NoDataString);
		
		// or...
		
		displayService.Display((await proxy.GetMachineDataOrNull())?.ToString() ?? NoDataString);
		
		// or...

		var dataString = NoDataString;
		try
		{
			dataString = (await proxy.GetMachineDataOrThrow()).ToString();
		}
		catch (Exception)
		{
			// ignored, since we just use a default value
		}
		displayService.Display(dataString);
	}
}