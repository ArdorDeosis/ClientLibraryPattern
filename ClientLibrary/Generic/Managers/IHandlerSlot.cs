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
  ValueTask<Result> Register(THandler handler);

  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  ValueTask<Result> Register(Func<THandler> handlerFactoryMethod);

  /// <summary>
  /// Registers a handler factory method.
  /// </summary>
  ValueTask<Result> Register(Func<ValueTask<THandler>> handlerFactoryMethod);

  /// <summary>
  /// Registers a handler factory.
  /// </summary>
  ValueTask<Result> Register(IFactory<THandler> handlerFactory);

  /// <summary>
  /// Deregisters the current handler factory.
  /// </summary>
  ValueTask<Result> Deregister();

  /// <summary>
  /// Changes the current handler factory to the given handler.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler no events are
  /// lost. The events occuring between deregistration and registration are buffered and handled by the new handler.
  /// </remarks>
  ValueTask<Result> Change(THandler handler);

  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  ValueTask<Result> Change(Func<THandler> handlerFactoryMethod);

  /// <summary>
  /// Changes the current handler factory to the given handler factory method.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory method no
  /// events are lost. The events occuring between deregistration and registration are buffered and handled by the new
  /// handler factory method.
  /// </remarks>
  ValueTask<Result> Change(Func<ValueTask<THandler>> handlerFactoryMethod);

  /// <summary>
  /// Changes the current handler factory to the given handler factory.
  /// </summary>
  /// <remarks>
  /// This ensures that between deregistration of the old factory and registration of the new handler factory no events
  /// are lost. The events occuring between deregistration and registration are buffered and handled by the new handler
  /// factory.
  /// </remarks>
  ValueTask<Result> Change(IFactory<THandler> handlerFactory);
}