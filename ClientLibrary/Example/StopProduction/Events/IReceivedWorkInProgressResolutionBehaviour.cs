namespace ClientLibrary;

public interface IReceivedWorkInProgressResolutionBehaviour : IStopProductionProcessEvent
{
	WorkInProgressResolutionBehaviour Behaviour { get; }
	ResponseType ResponseType { get; }
}