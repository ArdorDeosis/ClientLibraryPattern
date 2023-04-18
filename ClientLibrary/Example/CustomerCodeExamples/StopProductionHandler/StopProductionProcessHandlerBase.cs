namespace ClientLibrary;

public abstract class StopProductionProcessHandlerBase
{
	protected Guid ProcessId { get; private set; }
	protected StopProductionProcessState State { get; private set; } = StopProductionProcessState.NotStarted;
	
	/// <summary>
	/// Provides a resolution behaviour for a current product.
	/// </summary>
	protected abstract Task<OccupiedChamberResolutionBehaviour> ResolveOccupiedChamber(Data data, CancellationToken cancellationToken);

	/// <summary>
	/// Fired whenever the state changes.
	/// </summary>
	protected virtual void OnStateChanged(StopProductionProcessState state) { }

	/// <summary>
	/// Fired when the production stop process finished. The finishing state can be Completed or Aborted.
	/// </summary>
	protected virtual void OnProductionStopCompleted(StopProductionProcessState state) { }

	/// <summary>
	/// Aborts the current product, if the process is currently waiting for it to be finished.
	/// </summary>
	/// <returns>Whether the currently produced product has been aborted.</returns>
	protected Task<Result> AbortCurrentProduct(CancellationToken cancellationToken = default) => Task.FromResult(Result.Success);

	/// <summary>
	/// Cancels the production stop process.
	/// </summary>
	/// <returns>Whether the process could be stopped or not.</returns>
	protected Task<Result> CancelProductionStop(CancellationToken cancellationToken = default) => Task.FromResult(Result.Success);
}