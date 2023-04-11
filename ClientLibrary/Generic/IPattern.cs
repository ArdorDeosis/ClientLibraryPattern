namespace ClientLibrary;

public interface IPattern
{
  /// <summary>
  /// Simple request-response type action.
  /// </summary>
  /// <remarks>Could return a <see cref="ValueTask{TResult}"/></remarks>
  Task<Result> DoSomething();
  
  /// <summary>
  /// Simple request-response type action returning data.
  /// </summary>
  /// <remarks>Could return a <see cref="ValueTask{TResult}"/></remarks>
  Task<Result<Data>> GetData();
  
  /// <summary>
  /// Subscribing to an event.
  /// </summary>
  /// <param name="eventHandler">The event handler to be called when the event is fired.</param>
  /// <returns>
  /// An <see cref="IDisposable"/> that when it is disposed, informs the client library
  /// that the client unsubscribed.
  /// </returns>
  IDisposable SubscribeToEvent(Action eventHandler);
  
  /// <summary>
  /// Subscribing to an event with data.
  /// </summary>
  /// <param name="eventHandler">The event handler to be called when the event is fired.</param>
  /// <returns>
  /// An <see cref="IDisposable"/> that when it is disposed, informs the client library
  /// that the client unsubscribed.
  /// </returns>
  IDisposable SubscribeToEventWithData(Action<Data> eventHandler);
  
  /// <summary>
  /// Starts a process on the server that can be observed by the client. The client can react to status
  /// updates and make decisions based on them.
  /// </summary>
  /// <returns></returns>
  Task<Result<IProcessHandle>> StartProcess();
  
  /// <summary>
  /// Subscribes to processes or certain events in processes not started by this client. Receives the
  /// corresponding process handle to interact with these processes.
  /// </summary>
  /// <param name="processHandler"></param>
  /// <returns></returns>
  IDisposable SubscribeToProcess(Action<IProcessHandle> processHandler);
}