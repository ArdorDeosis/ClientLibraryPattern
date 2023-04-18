// ReSharper disable InconsistentNaming

namespace ClientLibrary;

public sealed class AlwaysWait_StopProductionProcessHandler : StopProductionProcessHandlerBase
{
	protected override Task<OccupiedChamberResolutionBehaviour> ResolveOccupiedChamber(Data data,
		CancellationToken cancellationToken) =>
		Task.FromResult(OccupiedChamberResolutionBehaviour.FinishCurrentProduct);
}



public sealed class AskAnotherService_StopProductionProcessHandler : StopProductionProcessHandlerBase
{
	protected override async Task<OccupiedChamberResolutionBehaviour> ResolveOccupiedChamber(Data data,
		CancellationToken cancellationToken) =>
		await SomeWebService.GetSome<OccupiedChamberResolutionBehaviour>(cancellationToken);
}



public sealed class WaitButAbortAfterTimeout_StopProductionProcessHandler : StopProductionProcessHandlerBase,
	IDisposable
{
	private readonly TimeSpan timeout;
	private Task? CancellationTask;

	public WaitButAbortAfterTimeout_StopProductionProcessHandler(TimeSpan timeout)
	{
		this.timeout = timeout;
	}

	public void Dispose()
	{
		CancellationTask?.Dispose();
	}

	protected override Task<OccupiedChamberResolutionBehaviour> ResolveOccupiedChamber(Data data,
		CancellationToken cancellationToken)
	{
		CancellationTask = CancelAfterTimeout(cancellationToken);
		return Task.FromResult(OccupiedChamberResolutionBehaviour.FinishCurrentProduct);
	}

	private async Task CancelAfterTimeout(CancellationToken externalCancellationToken)
	{
		await Task.Delay(timeout, externalCancellationToken);
		if (externalCancellationToken.IsCancellationRequested || State is not StopProductionProcessState.WaitingForProductFinished)
			return;
		var result = await AbortCurrentProduct(externalCancellationToken);
		if (result.IsFailure)
		{
			// error handling
		}
	}
}



public sealed class WaitAndListenToCancelRequests_StopProductionProcessHandler : StopProductionProcessHandlerBase
{
	protected override Task<OccupiedChamberResolutionBehaviour> ResolveOccupiedChamber(Data data,
		CancellationToken cancellationToken) => Task.FromResult(OccupiedChamberResolutionBehaviour.FinishCurrentProduct);

	public async Task<Result> CancelProductionStop() => await base.CancelProductionStop();
}
