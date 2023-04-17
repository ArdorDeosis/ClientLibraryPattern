// ReSharper disable InconsistentNaming
namespace ClientLibrary;

// Example interaction methods of a process handle.
public interface IProcessHandleExample : IProcessHandle
{
  // List of all events that have occurred (ReplaySubject).
  // We can save all events for a process, since processes usually do not last long enough to cause memory problems.
  IObservable<IProcessEvent> Events { get; }
  
  // An event that is guaranteed to happen exactly once could be offered as Task to make it awaitable.
  // E.g. process completion.
  Task ProcessCompleted { get; }

  // Interact with the process.
  Task<Result> InteractWithProcess();
}