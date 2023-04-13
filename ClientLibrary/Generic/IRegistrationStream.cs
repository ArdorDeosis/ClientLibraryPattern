namespace ClientLibrary;

public interface IRegistrationStream<out TData, TActivationParameters> where TActivationParameters : struct
{
  bool IsActive { get; }
  IObservable<TData>? Stream { get; }
  TActivationParameters? ActivationParameters { get; }
  ValueTask<Result> Register(TActivationParameters? parameters = null);
  ValueTask<Result> Deregister();
}