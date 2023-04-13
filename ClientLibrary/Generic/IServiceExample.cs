namespace ClientLibrary;

// Every return value is wrapped in a Task<Result<T>>. The Task gives us the opportunity to work async in the future,
// even if the operation currently is synchronous. The Result is the best way to handle errors, even if the operation 
// currently cannot fail.
// 
// All Task-returning members could be changed to return ValueTasks instead.
public interface IServiceExample
{
	#region Requests & Commands
	
	// Simple request-response type action.
	// Client sends command and receives a result as response.
	Task<Result> DoSomething();

	// Simple request-response type action returning data.
	// Client requests data and receives a result containing the requested data (if successful).
	Task<Result<Data>> GetData();

	#endregion

	#region Event Streams
	
	// Registrations are used when the server changes its behaviour, this would include sending data it did not send
	// before. Thus, if no registration is used, the data is already sent to the client and can be subscribed to without
	// async behaviour or failure.
	IObservable<Data> EventStream { get; }
	
	// Compared to the property above, this way provides opportunity to add parameters, e.g. filters.
	// Again, this is not async since no communication with the server is done.
	// NOTE: we probably don't need these and should offer extension methods on top to do filtering.
	Result<IObservable<Data>> GetEventStream();
	
	// NOTE: we should think about designing these as if they communicated with the server, to keep this option open
	
	// Pair of register method and event stream. For cases where the server needs to be informed to send data.
	IObservable<Data>? SpecialEventStream { get; }
	Task<Result<IObservable<Data>>> RegisterForSpecialEventStream();
	Task<Result> DeregisterForSpecialEventStream();
	
	// Alternative: this could be turned into an ActivatableStream type; see below
	
	// method for getting event streams filtered server-side
	// every call provides its own observable which has to be unregistered via the returned registration
	Task<Result<IRegistration<Data>>> RegisterFilteredEventStream(params object[] filter);
	
	#endregion
	
	#region Processes

	// Starts a single process on the server and provides a handler for interaction with the process.
	// This is basically a command providing a process handler.
	Task<Result> StartProcess(ProcessHandler handler);
	
	// Registers a process handler for processes not started by this client. In general, a client has only ever one
	// handler of a certain type, multi-handler functionality has to run through one handler interface.
	Task<Result> RegisterProcessHandler(ProcessHandler handler);
	Task<Result> RegisterProcessHandler(Func<ProcessHandler> handlerFactoryMethod);
	Task<Result> RegisterProcessHandler(IFactory<ProcessHandler> handlerFactory);
	
	// Deregisters the registered process handle.
	Task<Result> DeregisterProcessHandler();
	
	// This one guarantees that no event falls through between deregistration and registration. All events in between are
	// buffered and handled by the new handler. 
	Task<Result> ChangeProcessHandler(ProcessHandler newHandler);
	Task<Result> ChangeProcessHandler(Func<ProcessHandler> handlerFactoryMethod);
	Task<Result> ChangeProcessHandler(IFactory<ProcessHandler> handlerFactory);
	
	// Alternative: registration and deregistration methods could be turned into a ProcessHandleSlot type; see below 

	#endregion
}