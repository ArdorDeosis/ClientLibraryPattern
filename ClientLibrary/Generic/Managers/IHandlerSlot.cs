using JetBrains.Annotations;

namespace ClientLibrary;

/// <summary>
/// A registration slot for a handler factory.
/// </summary>
[PublicAPI]
// public interface IHandlerEndpoint<THandler>
// public interface IHandlerRegistry<THandler>
public interface IHandlerSlot<THandler>
{
  /// <summary>
  /// Whether a handler factory is registered.
  /// </summary>
  bool IsActive { get; }

  /// <summary>
  /// Registers a handler.
  /// </summary>
  // NOTE: depending on in which direction we go reg. multi-process.handling, this might not be possible. 
  Task<Result> Register(THandler handler);

  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  Task<Result> Register(Func<THandler> handlerFactoryMethod);

  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  Task<Result> Register(Func<Task<THandler>> handlerFactoryMethod);

  /// <summary>
  /// Registers a handler factory.
  /// </summary>
  Task<Result> Register(IFactory<THandler> handlerFactory);

  /// <summary>
  /// Deregisters the current handler factory.
  /// </summary>
  Task<Result> Deregister();

  /// <summary>
  /// Changes the current handler factory to the given handler.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler no events are
  /// lost. The events occuring between deregistration and registration are buffered and handled by the new handler.
  /// </remarks>
  // NOTE: see single-handler method above
  Task<Result> Change(THandler handler);

  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  Task<Result> Change(Func<THandler> handlerFactoryMethod);

  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  Task<Result> Change(Func<Task<THandler>> handlerFactoryMethod);

  /// <summary>
  /// Changes the current handler factory to the given handler factory.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory no events
  /// are lost. The events occuring between deregistration and registration are buffered and handled by the new handler
  /// factory.
  /// </remarks>
  Task<Result> Change(IFactory<THandler> handlerFactory);
}