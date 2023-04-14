using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;

namespace ClientLibrary;

/// <inheritdoc />
internal abstract class ActivationStream<TData> : IActivationStream<TData>
{
  private ISubject<TData>? subject;

  /// <inheritdoc />
  [MemberNotNullWhen(true, nameof(subject))]
  [MemberNotNullWhen(true, nameof(Stream))]
  public bool IsActive { get; private set; }

  /// <inheritdoc />
  public IObservable<TData>? Stream => subject;

  /// <inheritdoc />
  public async Task<Result> Activate()
  {
    if (IsActive)
      return "stream is already active";
    var result = await RegisterStream();
    if (result.IsFailure)
      return result;
    subject = result.Value;
    IsActive = true;
    return Result.Success;
  }

  /// <inheritdoc />
  public async Task<Result> Deactivate()
  {
    if (!IsActive)
      return "stream is not active";
    var result = await DeregisterStream();
    if (result.IsFailure)
      return result;
    subject.OnCompleted();
    subject = null;
    IsActive = false;
    return Result.Success;
  }

  private protected abstract Task<Result<ISubject<TData>>> RegisterStream();
  private protected abstract Task<Result> DeregisterStream();
}

/// <inheritdoc />
internal abstract class ActivationStream<TData, TActivationParameters> : IActivationStream<TData, TActivationParameters> 
  where TActivationParameters : struct
{
  private ISubject<TData>? subject;

  /// <inheritdoc cref="IActivationStream{TData,TActivationParameters}.IsActive" />
  [MemberNotNullWhen(true, nameof(subject))]
  [MemberNotNullWhen(true, nameof(Stream))]
  [MemberNotNullWhen(true, nameof(ActivationParameters))]
  public bool IsActive { get; private set; }

  /// <inheritdoc />
  public abstract TActivationParameters DefaultActivationParameters { get; }

  /// <inheritdoc />
  public IObservable<TData>? Stream => subject;

  /// <inheritdoc />
  public TActivationParameters? ActivationParameters { get; private set; }

  /// <inheritdoc cref="IActivationStream{TData,TActivationParameters}.Activate()" />
  public async Task<Result> Activate() => await Activate(DefaultActivationParameters);

  public async Task<Result> Activate(TActivationParameters parameters)
  {
    if (IsActive)
      return "stream is already active";
    var result = await RegisterStream(parameters);
    if (result.IsFailure)
      return result;
    subject = result.Value;
    IsActive = true;
    return Result.Success;
  }

  /// <inheritdoc />
  public async Task<Result> Deactivate()
  {
    if (!IsActive)
      return "stream is not active";
    var result = await DeregisterStream();
    if (result.IsFailure)
      return result;
    subject.OnCompleted();
    subject = null;
    IsActive = false;
    return Result.Success;
  }

  private protected abstract Task<Result<ISubject<TData>>> RegisterStream(TActivationParameters parameters);
  private protected abstract Task<Result> DeregisterStream();
}