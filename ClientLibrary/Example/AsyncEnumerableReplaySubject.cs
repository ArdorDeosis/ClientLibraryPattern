namespace ClientLibrary;

/// <summary>
/// An async enumerable representing an async data stream. Every time it is enumerated, it will replay all data that
/// it has collected so far, but the enumeration will wait for new data to be pushed to the subject unless the subject
/// terminated.
/// </summary>
public interface ITerminatingAsyncEnumerable<out T> : IAsyncEnumerable<T>
{
  /// <summary>
  /// Whether the subject has been terminated.
  /// </summary>
  bool HasTerminated { get; }
}


public interface IManualTerminatingAsyncEnumerable<T> : ITerminatingAsyncEnumerable<T>
{
  /// <summary>
  /// Pushes a new item to the subject.
  /// </summary>
  /// <exception cref="InvalidOperationException">When the subject has terminated.</exception>
  void Push(T item);

  /// <summary>
  /// Terminates the subject.
  /// </summary>
  /// <exception cref="InvalidOperationException">When the subject already has terminated.</exception>
  void Terminate();
}

/// <summary>
/// An async enumerable that collects data until it is terminated.
/// </summary>
public class ManualTerminatingAsyncEnumerable<T> : IManualTerminatingAsyncEnumerable<T>
{
  /// <summary>
  /// Whether the subject has been terminated.
  /// </summary>
  public bool HasTerminated { get; private set; }
  
  private readonly List<T> data = new();

  public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => 
    new AsyncEnumerator(this);

  /// <summary>
  /// Pushes a new item to the subject.
  /// </summary>
  /// <exception cref="InvalidOperationException">When the subject has terminated.</exception>
  public void Push(T item)
  {
    lock (data)
    {
      if (HasTerminated)
        throw new InvalidOperationException("Cannot push number after termination.");
      data.Add(item);
      Monitor.PulseAll(data);
    }
  }
    
  /// <summary>
  /// Terminates the subject.
  /// </summary>
  /// <exception cref="InvalidOperationException">When the subject already has terminated.</exception>
  public void Terminate()
  {
    lock (data)
    {
      if (HasTerminated)
        throw new InvalidOperationException("Cannot terminate twice.");
      HasTerminated = true;
      Monitor.PulseAll(data);
    }
  }
    
  /// <summary>
  /// An enumerator for this subject.
  /// </summary>
  private class AsyncEnumerator : IAsyncEnumerator<T>
  {
    private readonly ManualTerminatingAsyncEnumerable<T> source;
    private int pointer = -1;
    
    public AsyncEnumerator(ManualTerminatingAsyncEnumerable<T> source)
    {
      this.source = source;
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    /// <inheritdoc />
    public ValueTask<bool> MoveNextAsync()
    {
      lock (source.data)
      {
        pointer++;
        while (pointer >= source.data.Count && !source.HasTerminated)
          Monitor.Wait(source.data);
        return ValueTask.FromResult(pointer < source.data.Count || !source.HasTerminated);
      }
    }

    /// <inheritdoc />
    public T Current => source.data[pointer];
  }
}