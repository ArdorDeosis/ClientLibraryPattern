namespace ClientLibrary;

// This is an alternative idea: using interfaces to handle complex processes. Clients can then just implement these
// interfaces and provide the process registration methods with handlers.
public interface IServiceExampleAlternative
{

	#region Event Streams
	
	// TODO: do we need handlers here?
	Task<Result<IRegistration<Data>>> RegisterForEventStream();

	#endregion
	
	#region Processes

	// Starts a process on the server and returns a process handle the client can use to listen and react to status
	// updates and intercept the process.
	// This is basically a request returning a process handle.
	Task<Result> StartProcess(IProcessHandler handler);
	
	Task<Result> StartProcess(bool value);
	Task<Result> StartProcess(Func<QualityScanData, Task<bool>> value);
	Task<Result> StartProcess(QualityCheckHandler handler);
	
	// Registers for processes or certain events in processes not started by this client. Receives the corresponding
	// process handles to interact with these processes.
	// Basically like registering for an event stream, but receiving process handles.
	Task<Result<IRegistration<IProcessHandle>>> RegisterForProcessStream();

	#endregion
}