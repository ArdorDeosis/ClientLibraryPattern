namespace ClientLibrary;

// A registration at the server.
//
// This is done with the intention to clearly highlight the fact that the client is registered at the server, which may
// or may not change the servers behaviour. While the ignoring an event stream the client subscribed to does not change
// the behaviour of the server, ignoring the process handles in a registration's stream could be problematic, since the
// server might wait for a response.
//
// Note: Saving all past processes might lead to memory problems.
//
// TProcessHandle could (should?) implement IProcessHandle.
public interface IRegistration<out TProcessHandle>
{
	bool IsActive { get; }
	Task<Result> Unregister();

	// OPTION 1 - only one stream with fixed filter (most likely all open processes)
	IObservable<TProcessHandle> ProcessStream { get; }

	// OPTION 2 - filter options as parameters
	IObservable<TProcessHandle> GetProcessStream(params object[] options);

	// OPTION 3 - multiple fixed streams (could be shortcuts for option 2)
	IObservable<TProcessHandle> OpenProcesses { get; }
	IObservable<TProcessHandle> EndedProcesses { get; }
	IObservable<TProcessHandle> FutureProcesses { get; }
}

// Alternative to IRegistration
// basically option 1 from above
public interface IStreamRegistration<out TData> : IObservable<TData> {
	bool IsActive { get; }
	Task<Result> Unregister();
}

// Another alternative: represent each stream by an activatable stream? stream registration? stream connection???
// pros:
// - stream activation and deactivation are centralized
// cons:
// - streams are fixed, we could not work with parameters
public interface IStreamConnection<out T>
{
	bool IsActive { get; }
	Task<Result> Activate();
	Task<Result> Deactivate();
	IObservable<T>? Stream { get; }
}  