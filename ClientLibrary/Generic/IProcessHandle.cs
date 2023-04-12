namespace ClientLibrary;

// A generic process handle. Provides an ID that can be used to identify the process.
// This can have subscription methods and methods to interact with the specific process.
public interface IProcessHandle
{
  Guid ProcessId { get; }
  
  ProcessState State { get; }
  
  // This could be a behaviour subject always sending the current state on subscription.
  IObservable<ProcessState> ObservableState { get; }
}

// TODO: should this be part of the top-level process handle?
// potentially every process could fail; we could follow a similar wording as on the task class
public interface IResultProcessHandle<TResult> : IProcessHandle
{
  Task<TResult> Result { get; }
  IObservable<TResult> ResultStream { get; }
}

public enum ProcessState
{
  Unknown,
  Running,
  Completed,
}