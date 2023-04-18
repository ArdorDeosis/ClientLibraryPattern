namespace ClientLibrary;

public enum StopProductionProcessState
{
	Unknown,
	NotStarted,
	Started,
	WaitingForOccupiedChamberResolutionBehaviour,
	WaitingForProductFinished,
	WaitingForProductAborted,
	Completed,
	Aborted,
}