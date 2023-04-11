// ReSharper disable InconsistentNaming
namespace ClientLibrary;

public interface IProcessHandleExample : IProcessHandle
{
  /// <summary>
  /// List of all events that have occurred.
  /// </summary>
  IReadOnlyList<IProcessEvent> Events { get; }
  
  /// <summary>
  /// Combined event stream of state updates and decision requests.
  /// </summary>
  event Action<IProcessEvent> EventOccurred; // could be a replay subject
  
  // Alternative event subscription method. This would make it more consistent across the whole interface and open the
  // opportunity to add e.g. filter capabilities.
  IDisposable SubscribeToEvents(Action<IProcessEvent> eventHandler, params Type[] eventTypes);
  
  // Alternative event stream (optionally replaying all events).
  IAsyncEnumerable<IProcessEvent> EventStream { get; }

  // Alternative event stream with options reg. replay.
  IAsyncEnumerable<IProcessEvent> EventStreamWithOptions(EventOccurence occurence, params Type[] eventTypes);
  
  // NOTE: instead of IAsyncEnumerables we could also use IObservables

  // An event that is guaranteed to happen exactly once could be offered as Task to make it awaitable.
  // E.g. process completion.
  Task Process { get; }

  /// <summary>
  /// Interact with the process.
  /// </summary>
  Task<Result> InteractWithProcess();
}

public enum ApprovalStatus
{
  Approved,
  Rejected,
}

[Flags]
public enum EventOccurence
{
  None = 0,
  Past,
  Future,
}