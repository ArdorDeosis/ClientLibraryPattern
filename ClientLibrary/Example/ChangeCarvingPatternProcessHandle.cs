using System.Globalization;

namespace ClientLibrary;

public interface IChangeCarvingPatternProcessHandle : IProcessHandle
{
  /// <summary>
  /// The carving pattern to change to.
  /// </summary>
  CarvingPattern CarvingPattern { get; }
  
  /// <summary>
  /// The current status of the process.
  /// </summary>
  ChangeCarvingPatternProcessStatus Status { get; }
  
  /// <summary>
  /// All events that have happened during the process.
  /// </summary>
  IReadOnlyList<IChangeCarvingPatternProcessStatusUpdate> Events { get; }
  
  /// <summary>
  /// Event stream to handle all status updates.
  /// </summary>
  IAsyncEnumerable<IChangeCarvingPatternProcessStatusUpdate> EventStream { get; }

  /// <summary>
  /// Stream of all status update events.
  /// </summary>
  IDisposable SubscribeToStatusChangedEvents(Action<IChangeCarvingPatternProcessStatusUpdate> eventHandler);
  
  /// <summary>
  /// Stream of all clearance request events.
  /// </summary>
  IDisposable SubscribeToClearanceRequests(Action<IConfirmStationEmptyRequestHandle> eventHandler);
  
  /// <summary>
  /// Stream of all carving head change request events.
  /// </summary>
  IDisposable SubscribeToCarvingHeadChangeRequests(Action<ICarvingHeadChangeRequestHandle> eventHandler);
}

public enum ChangeCarvingPatternProcessStatus {
  NotStarted,
  LoadingPattern,
  WaitingForClearance,
  WaitingForCarvingHeadChange,
  Finished,
  Failed,
}

public interface IChangeCarvingPatternProcessStatusUpdate
{
  public ChangeCarvingPatternProcessStatus NewStatus { get; }
}

public interface IConfirmStationEmptyRequestHandle : IChangeCarvingPatternProcessStatusUpdate
{
  public bool HasBeenAnswered { get; }
  public Result Confirm();
}

public interface ICarvingHeadChangeRequestHandle : IChangeCarvingPatternProcessStatusUpdate
{
  public bool HasBeenAnswered { get; }
  public CarvingHead RequestedCarvingHead { get; }
  public Result Confirm();
}

public interface ITestProcessHandle
{
  IAsyncEnumerable<int> Numbers { get; }
  void SendNumber(int number);
}

public class TestProcessHandleImpl : ITestProcessHandle
{
  private readonly object lockObject = new();
  private readonly List<int> numbers = new();
  private TaskCompletionSource<int>? nextNumberTaskCompletionSource = new();

  public IAsyncEnumerable<int> Numbers => GetNumbersAsync();
  
  private async IAsyncEnumerable<int> GetNumbersAsync()
  {
    foreach (var number in numbers.ToArray())
      yield return number;
    while (nextNumberTaskCompletionSource is not null)
    {
      var newNumber = await nextNumberTaskCompletionSource.Task;
      yield return newNumber; 
    }
  }

  public void SendNumber(int number)
  {
    if (nextNumberTaskCompletionSource is null) throw new InvalidOperationException();
    numbers.Add(number);
    var lastTaskCompletionSource = nextNumberTaskCompletionSource;
    nextNumberTaskCompletionSource = number is 0 ? null : new TaskCompletionSource<int>();
    lastTaskCompletionSource.SetResult(number);
  }
}