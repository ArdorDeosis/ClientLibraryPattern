namespace ClientLibrary;

/// <summary>
/// A registration slot for a handler factory.
/// </summary>
// public interface IHandlerEndpoint<in THandler>
// public interface IHandlerRegistry<in THandler>
public abstract class HandlerSlot<THandler>
{
  /// <summary>
  /// Whether a handler factory is registered.
  /// </summary>
  public bool IsActive { get; }
  
  /// <summary>
  /// Registers a handler.
  /// </summary>
  public ValueTask<Result> Register(THandler handler) => 
    Register(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handler)));

  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  public ValueTask<Result> Register(Func<THandler> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handlerFactoryMethod())));
  
  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  public ValueTask<Result> Register(Func<ValueTask<THandler>> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <summary>
  /// Registers a handler factory.
  /// </summary>
  public abstract ValueTask<Result> Register(IFactory<THandler> handlerFactory);

  /// <summary>
  /// Deregisters the current handler factory.
  /// </summary>
  public abstract ValueTask<Result> Deregister();

  /// <summary>
  /// Changes the current handler factory to the given handler.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler no events are
  /// lost. The events occuring between deregistration and registration are buffered and handled by the new handler.
  /// </remarks>
  public ValueTask<Result> Change(THandler handler) => 
    Change(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handler)));

  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  public ValueTask<Result> Change(Func<THandler> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handlerFactoryMethod())));
  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  public ValueTask<Result> Change(Func<ValueTask<THandler>> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <summary>
  /// Changes the current handler factory to the given handler factory.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory no events
  /// are lost. The events occuring between deregistration and registration are buffered and handled by the new handler
  /// factory.
  /// </remarks>
  public abstract ValueTask<Result> Change(IFactory<THandler> handlerFactory);
}