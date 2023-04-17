namespace ClientLibrary;

public enum StopProductionProcessStatus
{
	Unknown,
	StartedProcess,
	WaitingForWorkInProgressResolutionBehaviour,
	WaitingForEmptyChamber,
	Completed,
	Aborted,
}