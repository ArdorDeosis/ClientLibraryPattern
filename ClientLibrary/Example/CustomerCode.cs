using System.Reactive.Linq;

namespace ClientLibrary;

/// <summary>
/// Example usages of the <see cref="IPokeballCarvingStationClient"/>
/// </summary>
internal partial class CustomerCode : IDisposable
{
  private readonly IPokeballCarvingStationClient client;
  
  // TODO: do I still need this?
  private readonly HashSet<IDisposable> subscriptions = new();
  
  private IRegistration<IQualityCheckInterceptionProcessHandle>? qualityCheckInterceptionRegistration;

  public CustomerCode(IPokeballCarvingStationClient client)
  {
    this.client = client;
    
    subscriptions.Add(client.ObservableState.Subscribe(HandleMachineStateChanged));
  }

  public void Dispose()
  {
    foreach (var subscription in subscriptions) 
      subscription.Dispose();
  }

  internal async Task<Result> StartInterceptingQualityChecks()
  { 
     var result = await client.RegisterQualityCheckInterceptor();
     if (result.IsFailure) 
       return $"could not register quality check interceptor: {result.ErrorMessage}";
     qualityCheckInterceptionRegistration = result.Value;
     subscriptions.Add(qualityCheckInterceptionRegistration.OpenProcesses.Subscribe(HandleQualityCheckInterception));
     return Result.Success;
  }

  internal async Task<Result> StopInterceptingQualityChecks()
  {
    if (qualityCheckInterceptionRegistration is null)
      return "not registered to intercept quality checks";
    var result = await qualityCheckInterceptionRegistration.Unregister();
    return result.IsFailure 
      ? $"could not deregister quality check interceptor: {result.ErrorMessage}" 
      : Result.Success;
  }

  internal async Task<Result> StopMachine()
  {
    var getHandleResult = await client.StopProduction();
    if (getHandleResult.IsFailure)
      return $"could not stop production: {getHandleResult.ErrorMessage}";
    var handle = getHandleResult.Value;

    using var subscription = handle.Events
      .OfType<IRequestWorkInProgressResolutionBehaviour>()
      .Subscribe(async @event =>
    {
      DoStuffWith(@event.ProductionData);
      await @event.Answer(WorkInProgressResolutionBehaviour.AbortCurrentProduct);
    });

    return await handle.Result;
  }

  internal async Task StartMachine()
  {
    var result = await client.StartProduction();
    if (result.IsFailure)
      Console.WriteLine($"could not start production: {result.ErrorMessage}");
  }

  internal async Task PrintMachineId()
  {
    var result = await client.GetMachineId();
    if (result.IsFailure)
      Console.WriteLine($"could not retrieve machine ID: {result.ErrorMessage}");
    else
      Console.WriteLine(result.Value);
  }

  private async void HandleQualityCheckInterception(IQualityCheckInterceptionProcessHandle handle)
  {
    var result = await handle.Answer(CheckQuality(handle.Quality));
    if (result.IsFailure)
    {
      // log error message
      // maybe retry
    }
  }
}