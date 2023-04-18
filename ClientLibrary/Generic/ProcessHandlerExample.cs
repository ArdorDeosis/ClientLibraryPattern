namespace ClientLibrary;

public abstract class ProcessHandlerExample
{
	// STATE
	
	// state property
	// for sub-classes readonly
	protected Data SomeState { get; private set; }
	
	
	// EVENTS
	
	// optionally observables
	// opens up option for replay subjects etc.
	protected IObservable<Data> EventStream { get; private set; }
	
	// event model based on methods <== I quite like this one
	protected virtual void OnEventHappened(Data data) {}
	
	// TODO: should we allow async methods? we're not waiting on it anyways, I guess?
	protected virtual Task OnAnotherEventHappened(Data data) => Task.CompletedTask;

	
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

// Strategies for multi-process handling:
// 1. multi-process handlers, no factories
// 2. single-process handlers, only factories
// 3. single-process handlers, no factories, proxy queues processes <== best of both worlds?
//
// discussion:
// state-less process handlers could be no-factory/single-process, but how do we distinguish them?
// 
// pro single-process/factory:
// - handlers are simpler to write => no danger of introducing non-separated state
// 
// pro multi-process:
// - process handler can be injected into other classes for e.g. interrupts
// contra multi-process:
// - factory needs a Created event
//
// event options for process started events:
// - factory
// - handler-endpoint