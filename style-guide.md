# Proxy API Style Guide
A quick guide on how the PCC proxy API is designed. 

## Simple Requests & Commands
Simple request/response-pattern interactions follow this form:

```csharp
Task<Result> DoSomething(CancellationToken cancellationToken = default);  
Task<Result<Data>> GetData(CancellationToken cancellationToken = default);
```
Additional arguments can be provided before the `CancellationToken`.

### Why `Task`?
Even if a request is synchronous at the moment, returning a `Task<T>`  enables a later switch to an actually asynchronous operation without an API change.

### Why `Result`?

No interactions with the proxy should throw exceptions, thus all requests should return a form of result type ([example](https://gist.github.com/ArdorDeosis/7b22cee4fb449d697281c75dad2281cb)). Even if the action cannot fail right now, same as above, defining the signature this way enables us to later change the code in a way that it can fail without changing the signature and causing a breaking API change.

## Data Streams & Events
Event streams are provided as [`IObservable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=net-7.0) properties:
```csharp  
IObservable<Data> EventStream { get; }
```
### Why not just use native C# events?

Observables as defined in [ReactiveX](https://reactivex.io/)  are more flexible than native C# events. Additionally [Rx.NET](https://github.com/dotnet/reactive) offers an extensive library of methods and operators to simplify work with asynchronous data streams. (see [System.Reactive](https://www.nuget.org/packages/System.Reactive/) and [System.Interactive](https://www.nuget.org/packages/System.Interactive/)).

### Available on Subscription
Data streams that need to be activated on the server before they can be used are provided as `IActivatableStream<T>` (name may change):

```csharp
public interface IActivationStream<out TData>
{
	/// <summary>Whether the stream is active (and thus usable).</summary>
	[MemberNotNullWhen(true, nameof(Stream))]
	bool IsActive { get; }
	
	/// <summary>The data stream, if it is active, otherwise null.</summary>
	IObservable<TData>? Stream { get; }
	
	/// <summary>Activates the stream.</summary>
	Task<Result> Activate();
	
	/// <summary>Deactivates the stream.</summary>
	Task<Result> Deactivate();
}
```
An option taking input parameters for activation is possible, too.

## Process Interaction
Complex processes that offer interaction possibilities or require interaction are handled by process handlers. A process handler can always only handle a single process at a time. Processes triggered by the client provide a process handler factory.
```csharp
Task<Result<ProcessHandler>> StartProcess(IFactory<ProcessHandler> handlerFactory, CancellationToken cancellationToken = default);
```
Overloads are provided for convenience:
```csharp
Task<Result<ProcessHandler>> StartProcess(ProcessHandler handler, CancellationToken cancellationToken = default);
Task<Result<ProcessHandler>> StartProcess(Action<ProcessHandler> handlerFactoryMethod, CancellationToken cancellationToken = default);
Task<Result<ProcessHandler>> StartProcess(Action<Task<ProcessHandler>> handlerFactoryMethod, CancellationToken cancellationToken = default);
```
The methods return the used `ProcessHandler`.

### Why Factories?
Using factories behind the scenes (instead of a passed in handler instance) enables the proxy to check conditions and input parameters before creating the handler. This means in case the process can not be started no handler is created, and factories notifying other modules about created handlers are not called.
It also enables the user to decouple the handler creation from the process-triggering code, since a handler factory can be injected.

> Note: I thought about adding the possibility to register default process handler factories, which would decouple the handler definition and process-triggering code even more. I'd be happy to receive feedback on this topic.

### Registering Process Handlers for Server-Triggered Processes
For processes triggered by the server, the client can register process handler factories. These are registered at a `IProcessHandlerEndpoint<T>`.
```csharp
public interface IHandlerEndpoint<THandler>
{
	/// <summary>Whether a handler factory is registered.</summary>
	bool HasFactoryRegistered { get; }
	
	/// <summary>Registers a handler factory.</summary>
	Task<Result> Register(IFactory<THandler> handlerFactory);
	
	/// <summary>Deregisters the current handler factory.</summary>
	Task<Result> Deregister();
	
	/// <summary>Changes the current handler factory to the given handler.</summary>
	/// <remarks>
	/// This ensures that between deregistration of the old factory and registration
	/// of the new handler no events are lost.
	/// </remarks>
	Task<Result> Change(IFactory<THandler> handlerFactory);
}
```

## Process Handler Design
Process handlers are written by the user inheriting abstract implementations provided by the proxy. A process handler can always only handle a **single process at a time**.

> Note: I thought about implementing a generic interface for a process handler providing basic data about whether the process handler is currently working on a process or not, the process ID, event methods for generic process start and finish events etc.

***State*** is made available through protected properties:
```csharp
protected Data State { get; }
```
*Note: The proxy API should avoid state properties wherever possible and use input parameters for decision and event methods instead.*

***Data Streams*** are available through `IObservables<T>`. (see above)

***Event Methods*** can be overridden to react to certain events in the process.
```csharp
protected virtual void OnEventHappened(Data data) {}
```
***Decisions*** that need to be made by the process handler are implemented as abstract methods.
```csharp
protected abstract Task<Decision> MakeDecision(Data data, CancellationToken cancellationToken = default);
```
Virtual decision methods ensure better backwards-compatibility and can be used as optional decisions.
```csharp
protected virtual Task<Decision> MakeDecision(Data data, CancellationToken cancellationToken = default) => Task.FromResult(Decision.Deny);
```

***Interrupts.*** Process handlers offer protected methods for the implementation to interact with the base process.
```csharp
protected Task<Result> InterruptProcess(CancellationToken cancellationToken = default)
```

## Examples
### Generic Proxy Interface
```csharp
// Every return value is wrapped in a Task<Result<T>>. The Task gives us the opportunity to work async in the future,
// even if the operation currently is synchronous. The Result is the best way to handle errors, even if the operation 
// currently cannot fail.
// 
// Note: All Task-returning members could be changed to return ValueTasks instead.
public interface IProxy
{
	// REQUESTS & COMMANDS
	
	// Simple request-response type action.
	// Client sends command and receives a result as response.
	Task<Result> DoSomething(CancellationToken cancellationToken = default);

	// Simple request-response type action returning data.
	// Client requests data and receives a result containing the requested data (if successful).
	Task<Result<Data>> GetData(CancellationToken cancellationToken = default);

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
	Task<Result<ProcessHandler>> StartProcess(ProcessHandler handler, CancellationToken cancellationToken = default);
	Task<Result<ProcessHandler>> StartProcess(Action<ProcessHandler> handlerFactoryMethod, CancellationToken cancellationToken = default);
	Task<Result<ProcessHandler>> StartProcess(Action<Task<ProcessHandler>> handlerFactoryMethod, CancellationToken cancellationToken = default);
	Task<Result<ProcessHandler>> StartProcess(IFactory<ProcessHandler> handlerFactory, CancellationToken cancellationToken = default);
	
	// Slot for registering a process handler.
	IHandlerEndpoint<ProcessHandler> HandlerEndpoint { get; }
}
```
### Generic Process Handler Base
```csharp

public abstract class ProcessHandlerExample
{
	// STATE
	
	// state property
	// for sub-classes readonly
	protected Data State { get; private set; }
	
	
	// EVENTS
	
	// optionally observables
	// opens up option for replay subjects etc.
	protected IObservable<Data> DataStream { get; private set; }
	
	// event model based on methods <== I quite like this one
	protected virtual void OnEventHappened(Data data) {}
	
	
	// DECISION MAKING

	// mandatory decision
	protected abstract Task<Decision> MakeDecision(Data data, CancellationToken cancellationToken = default);
	
	// optional decision making
	// this optional decision making keeps us backwards compatible when adding new decisions
	protected virtual Task<Decision> MakeOptionalDecision(Data data, CancellationToken cancellationToken = default) => Task.FromResult(Decision.Deny);
	
	
	// INTERRUPTS

	// interrupt process
	protected Task<Result> InterruptProcess(CancellationToken cancellationToken = default) => Task.FromResult(Result.Success);
}
```

## Configuration
Some configuration will have to be done when a proxy instance is created. This configuration includes:
* Connection Parameters
* Ping / Healthcheck Intervals
* (Default) Timeout & Retry Policies
* Logging
	* 6 log levels should be provided: Trace, Debug, Info, Warning, Error, Fatal
	* multiple log targets can be registered

## Open Discussion
### Reconnect After Restart
We have no design yet on how to ensure that the client picks up open processes from the server after a restart.
Best solution might be to just offer an endpoint informing about open processes, so that the client can register the corresponding handlers.
