namespace ClientLibrary;

public interface IStopProductionProcessHandle : IResultProcessHandle<Result>
{
	public IObservable<IStopProductionProcessEvent> Events { get; }
}