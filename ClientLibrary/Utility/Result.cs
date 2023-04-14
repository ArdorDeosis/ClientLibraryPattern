using System.Diagnostics.CodeAnalysis;

namespace ClientLibrary;

/// <summary>
/// A result type not containing data. Provides information about whether an operation was successful or not and an
/// error message in case of failure.
/// </summary>
public sealed class Result
{
  /// <summary>
  /// The error message providing more detail about what went wrong.
  /// </summary>
  /// <remarks>This is only set if the result indicates an error otherwise it is null.</remarks>
  public string? ErrorMessage { get; private init; }

  /// <summary>
  /// Whether the result indicates a success.
  /// </summary>
  [MemberNotNullWhen(false, nameof(ErrorMessage))]
  public bool IsSuccess { get; private init; }

  /// <summary>
  /// Whether the result indicates an error.
  /// </summary>
  [MemberNotNullWhen(true, nameof(ErrorMessage))]
  public bool IsFailure => !IsSuccess;

  private Result() { }

  /// <summary>
  /// Implicit conversion of a string to a result indicating an error.
  /// </summary>
  /// <param name="errorMessage">The error message providing more information about what went wrong.</param>
  public static implicit operator Result(string errorMessage) => Failure(errorMessage);

  /// <summary>
  /// A result indicating a success.
  /// </summary>
  public static Result Success { get; } = new() { IsSuccess = true };

  /// <summary>
  /// A result indicating an error.
  /// </summary>
  /// <param name="errorMessage">The error message providing more information about what went wrong.</param>
  public static Result Failure(string errorMessage) => new() { ErrorMessage = errorMessage, IsSuccess = false };
}