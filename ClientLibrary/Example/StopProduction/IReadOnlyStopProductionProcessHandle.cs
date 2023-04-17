namespace ClientLibrary;

public interface IReadOnlyStopProductionProcessHandle : IProcessHandle
{
	public IObservable<IStopProductionProcessEvent> Events { get; }
}