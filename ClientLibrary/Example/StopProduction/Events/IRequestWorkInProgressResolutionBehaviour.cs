namespace ClientLibrary;

public interface IRequestWorkInProgressResolutionBehaviour :
	IDecisionRequest<WorkInProgressResolutionBehaviour>,
	IStopProductionProcessEvent
{
	public PokeballProductionData ProductionData { get; }
}