namespace ClientLibrary;

public interface IStopProductionProcessEvent
{
	StopProductionProcessStatus NextStatus { get; }
}