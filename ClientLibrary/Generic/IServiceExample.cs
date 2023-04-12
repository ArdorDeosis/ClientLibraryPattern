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
	Result<IObservable<Data>> GetEventStream();
	
	// TODO: is this necessary?
	// The idea behind the IRegistration object is to clearly convey to the user that this method does not just return an
	// event stream, but registers the client at the server potentially altering the servers behaviour. Could this be the
	// case for any read-only event streams?
	// 
	// The registration will be cached as long as an active one is present.
	Task<Result<IRegistration<Data>>> RegisterForEventStream();
	
	// Patterns would be imaginable in which a property provides potential registrations for easier access:
	IObservable<int>? SomeDataStream { get; }
	Task<Result<IRegistration<int>>> RegisterForSomeDataStream();

	#endregion
	
	#region Processes

	// Starts a process on the server and returns a process handle the client can use to listen and react to status
	// updates and intercept the process.
	// This is basically a request returning a process handle.
	Task<Result<IProcessHandle>> StartProcess();
	
	// Subscribes to processes or certain events in processes not started by this client. Receives the corresponding
	// process handle to interact with these processes.
	// Basically like subscribing to an event stream, but receiving process handles.
	//
	// This could also return all open processes of the type.
	// TODO: do we need this? or should we restrict processes to the registration process?
	Task<Result<IObservable<IProcessHandle>>> SubscribeToProcessStream();
	
	// Registers for processes or certain events in processes not started by this client. Receives the corresponding
	// process handles to interact with these processes.
	// Basically like registering for an event stream, but receiving process handles.
	Task<Result<IRegistration<IProcessHandle>>> RegisterForProcessStream();

	#endregion
}
