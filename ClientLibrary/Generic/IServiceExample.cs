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
	
	IActivationStream<Data> SpecialEventStream { get; }

	IActivationStream<Data, Parameters> ServerSideFilteredEventStream { get; }
	
	#endregion
	
	#region Processes

	// Starts a single process on the server and provides a handler for interaction with the process.
	// This is basically a command providing a process handler.
	Task<Result> StartProcess(IProcessHandler handler);
	
	// Slot for registering a process handler.
	IHandlerSlot<IProcessHandler> HandlerEndpoint { get; } 

	#endregion
}