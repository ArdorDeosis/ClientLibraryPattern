namespace ClientLibrary;

/// <inheritdoc />
// public abstract class HandlerEndpoint<THandler>
// public abstract class HandlerRegistry<THandler>
internal abstract class HandlerSlot<THandler> : IHandlerSlot<THandler>
{
  /// <inheritdoc />
  public bool IsActive { get; protected set; }

  /// <inheritdoc />
  public ValueTask<Result> Register(THandler handler) =>
    Register(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handler)));

  /// <inheritdoc />
  public ValueTask<Result> Register(Func<THandler> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handlerFactoryMethod())));

  /// <inheritdoc />
  public ValueTask<Result> Register(Func<ValueTask<THandler>> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <inheritdoc />
  public abstract ValueTask<Result> Register(IFactory<THandler> handlerFactory);

  /// <inheritdoc />
  public abstract ValueTask<Result> Deregister();

  /// <inheritdoc />
  public ValueTask<Result> Change(THandler handler) =>
    Change(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handler)));

  /// <inheritdoc />
  public ValueTask<Result> Change(Func<THandler> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(() => ValueTask.FromResult(handlerFactoryMethod())));

  /// <inheritdoc />
  public ValueTask<Result> Change(Func<ValueTask<THandler>> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <inheritdoc />
  public abstract ValueTask<Result> Change(IFactory<THandler> handlerFactory);
}