using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ClientLibrary;

/// <summary>
/// A stream that needs to be activated to be used.
/// </summary>
/// <typeparam name="TData">The type of data the stream produces.</typeparam>
[PublicAPI]
public interface IActivationStream<out TData>
{
  /// <summary>
  /// Whether the stream is active (and thus usable).
  /// </summary>
  [MemberNotNullWhen(true, nameof(Stream))]
  bool IsActive { get; }
  
  /// <summary>
  /// The data stream, if it is active, otherwise null.
  /// </summary>
  IObservable<TData>? Stream { get; }

  /// <summary>
  /// Activates the stream.
  /// </summary>
  Task<Result> Activate();

  /// <summary>
  /// Deactivates the stream.
  /// </summary>
  Task<Result> Deactivate();
}

/// <inheritdoc cref="IActivationStream{TData}"/>
/// <typeparam name="TActivationParameters">The type of the parameters used to activate the stream.</typeparam>
[PublicAPI]
public interface IActivationStream<out TData, TActivationParameters> : IActivationStream<TData> where TActivationParameters : struct
{
  /// <inheritdoc cref="IActivationStream{TData}.IsActive"/>
  [MemberNotNullWhen(false, nameof(DefaultActivationParameters))]
  new bool IsActive { get; }
  
  /// <summary>
  /// The default activation parameters used when no parameters are provided.
  /// </summary>
  TActivationParameters DefaultActivationParameters { get; }
  
  /// <summary>
  /// The activation parameters used to activate the currently active stream or null if the stream is not active.
  /// </summary>
  TActivationParameters? ActivationParameters { get; }
  
  /// <summary>
  /// Activates the stream using the <see cref="DefaultActivationParameters"/>.
  /// </summary>
  new Task<Result> Activate();
  
  /// <summary>
  /// Activates the stream using the provided parameters.
  /// </summary>
  /// <param name="parameters">The parameters used to activate the stream.</param>
  Task<Result> Activate(TActivationParameters parameters);
}