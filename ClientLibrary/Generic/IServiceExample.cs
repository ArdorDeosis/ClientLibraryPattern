namespace ClientLibrary;

// Every return value is wrapped in a Task<Result<T>>. The Task gives us the opportunity to work async in the future,
// even if the operation currently is synchronous. The Result is the best way to handle errors, even if the operation 
// currently cannot fail.
// 
// All Task-returning members could be changed to return Tasks instead.
public interface IServiceExample
{
	// REQUESTS & COMMANDS
	
	// Simple request-response type action.
	// Client sends command and receives a result as response.
	Task<Result> DoSomething(CancellationToken cancellationToken = default);
	Task<bool> TryDoSomething(CancellationToken cancellationToken = default);
	Task DoSomethingOrThrow(CancellationToken cancellationToken = default);

	// Simple request-response type action returning data.
	// Client requests data and receives a result containing the requested data (if successful).
	Task<Result<Data>> GetData(CancellationToken cancellationToken = default);
	Task<Data?> GetDataOrNull(CancellationToken cancellationToken = default);
	Task<Data> GetDataOrThrow(CancellationToken cancellationToken = default);

	// EVENT STREAMS
	
	// Registrations are used when the server changes its behaviour, this would include sending data it did not send
	// before. Thus, if no registration is used, the data is already sent to the client and can be subscribed to without
	// async behaviour or failure.
	IObservable<Data> EventStream { get; }
	
	// Event stream requiring activation. The IActivationStream provides methods for activating and deactivating the
	// stream. See IActivationStream for more detail. 
	IActivationStream<Data> SpecialEventStream { get; }

	// Same as above, but with input parameters for the activation.
	IActivationStream<Data, Parameters> ServerSideFilteredEventStream { get; }
	
	// PROCESSES

	// Starts a single process on the server and provides a handler for interaction with the process.
	// This is basically a command providing a process handler.
	Task<Result> StartProcess(ProcessHandlerExample handlerExample, CancellationToken cancellationToken = default);
	
	// Slot for registering a process handler.
	IHandlerSlot<ProcessHandlerExample> HandlerEndpoint { get; }
	
	// TODO: processes might be a problem in terms of error handling!
	// If the user has the possibility to just register and deregister handlers, the user might not register a type of
	// handler that has a running process after restart.
	// - implementation of a 'ask what processes are running' method could help, but feels very clumsy and relies on the
	// user to make the right decision.
	// - optionally we could move handler registration to the client creation, also not a nice solution
	// - for starting single processes this would mean they also need a registered handler factory
}


// Parameters for Creation:
// Logging
//		6 levels (Trace, Debug, Info, Warning, Error, Fatal)
//		multiple log targets possible
// Timeout and Retry
// Connection Parameters
// Ping / Healthcheck Intervals