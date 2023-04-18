namespace ClientLibrary;

// Scenario:
// A service that starts the machine when a button is pressed. Display a message during start and a message with the
// result (success, cancellation or error).
public sealed class MachineStartingService : IDisposable
{
	private readonly IPokeballCarvingStationProxy proxy;
	private readonly IDisplayService displayService;

	private MachineState machineState = MachineState.NotStarted;
	private CancellationTokenSource? cancellationTokenSource;

	public MachineStartingService(IPokeballCarvingStationProxy proxy, IDisplayService displayService)
	{
		this.proxy = proxy;
		this.displayService = displayService;
	}

	public void Dispose() => cancellationTokenSource?.Dispose();

	/// <summary>
	/// Executed when the button is pressed.
	/// </summary>
	private async Task OnButtonPressed()
	{
		#region State Guards

		switch (machineState)
		{
			case MachineState.Started:
				displayService.Display("machine is already running");
				return;
			case MachineState.Starting:
				displayService.Display("machine is already starting");
				return;
			case MachineState.Unknown:
				throw new Exception("machine state is unknown");
		}

		#endregion

		machineState = MachineState.Starting;
		cancellationTokenSource = new CancellationTokenSource();
		DisplayMachineStartingMessage();

		await StartMachine__1(cancellationTokenSource.Token);
		// or
		await StartMachine__2(cancellationTokenSource.Token);

		cancellationTokenSource?.Dispose();
		cancellationTokenSource = null;
	}

	private async Task StartMachine__1(CancellationToken token)
	{
		var result = await proxy.StartProduction(token);

		machineState = result.IsSuccess
			? MachineState.Started
			: MachineState.NotStarted;

		if (result.IsSuccess)
			displayService.Display("machine started!");
		else if (token.IsCancellationRequested)
			displayService.Display("machine start was cancelled!");
		else
			displayService.Display($"machine could not be started; {result.ErrorMessage}");
	}

	private async Task StartMachine__2(CancellationToken token)
	{
		try
		{
			await proxy.StartProductionOrThrow(token);
		}
		catch (OperationCanceledException)
		{
			machineState = MachineState.NotStarted;
			displayService.Display("machine start was cancelled");
		}
		catch (Exception exception)
		{
			machineState = MachineState.NotStarted;
			displayService.Display($"machine could not be started; {exception}");
		}
		machineState = MachineState.Started;
		displayService.Display("machine started!");
	}

	private void DisplayMachineStartingMessage() => displayService.Display(
		"starting machine",
		new ButtonDefinition(Title: "Cancel", Action: () => cancellationTokenSource?.Cancel()));
}

public enum MachineState
{
	Unknown,
	NotStarted,
	Starting,
	Started,
}