namespace ClientLibrary;

/// <summary>
/// A generic process handle. Provides an ID that can be used to identify the process.
/// </summary>
/// <remarks>
/// This can have subscription methods and methods to interact with the specific process.
/// </remarks>
public interface IProcessHandle
{
  Guid ProcessId { get; }
}

/// <summary>
/// Event that can occur in a process.
/// </summary>
/// <remarks>This is an empty marker interface and I'd prefer a different solution.</remarks>
public interface IProcessEvent {}


public class StatusUpdateEvent : IProcessEvent { /* placeholder */ }

/// <summary>
/// Example of a generic decision request event.
/// </summary>
public class DecisionRequestEvent<T> : IProcessEvent
{
  private T? answerValue;

  /// <summary>
  /// Whether the decision request has been answered.
  /// </summary>
  public bool HasBeenAnswered { get; private set; }

  /// <summary>
  /// The answer value. Throws an exception if the decision request has not been answered yet.
  /// </summary>
  public T? AnswerValue
  {
    get => HasBeenAnswered
      ? answerValue
      : throw new InvalidOperationException("The decision request has not been answered yet.");
    private set => answerValue = value;
  }

  /// <summary>
  /// Answers the decision request.
  /// </summary>
  public Result Answer(T answer)
  {
    // send answer to server
    AnswerValue = answer;
    HasBeenAnswered = true;
    return Result.Success;
  }
}