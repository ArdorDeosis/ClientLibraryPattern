namespace ClientLibrary;

/// <inheritdoc />
// public abstract class HandlerEndpoint<THandler>
// public abstract class HandlerRegistry<THandler>
internal abstract class HandlerSlot<THandler> : IHandlerSlot<THandler>
{
  /// <inheritdoc />
  public bool IsActive { get; protected set; }

  /// <inheritdoc />
  public Task<Result> Register(THandler handler) =>
    Register(new SimpleAsyncFactory<THandler>(() => Task.FromResult(handler)));

  /// <inheritdoc />
  public Task<Result> Register(Func<THandler> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(() => Task.FromResult(handlerFactoryMethod())));

  /// <inheritdoc />
  public Task<Result> Register(Func<Task<THandler>> handlerFactoryMethod) =>
    Register(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <inheritdoc />
  public abstract Task<Result> Register(IFactory<THandler> handlerFactory);

  /// <inheritdoc />
  public abstract Task<Result> Deregister();

  /// <inheritdoc />
  public Task<Result> Change(THandler handler) =>
    Change(new SimpleAsyncFactory<THandler>(() => Task.FromResult(handler)));

  /// <inheritdoc />
  public Task<Result> Change(Func<THandler> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(() => Task.FromResult(handlerFactoryMethod())));

  /// <inheritdoc />
  public Task<Result> Change(Func<Task<THandler>> handlerFactoryMethod) =>
    Change(new SimpleAsyncFactory<THandler>(handlerFactoryMethod));

  /// <inheritdoc />
  public abstract Task<Result> Change(IFactory<THandler> handlerFactory);
}