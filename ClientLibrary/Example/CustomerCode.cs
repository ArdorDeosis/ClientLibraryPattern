namespace ClientLibrary;

/// <summary>
/// Example usages of the <see cref="IPokeballCarvingStationClient"/>
/// </summary>
internal partial class CustomerCode : IDisposable
{
  private readonly IPokeballCarvingStationClient client;
  
  private readonly HashSet<IDisposable> subscriptions = new();

  public CustomerCode(IPokeballCarvingStationClient client)
  {
    this.client = client;
    
    subscriptions.Add(client.SubscribeToStateChanged(HandleMachineStateChanged));
    subscriptions.Add(client.SubscribeToPokeballFinished(HandlePokeballFinished));
    subscriptions.Add(client.RegisterQualityCheckInterceptor(HandleQualityCheckInterception));
  }

  public void Dispose()
  {
    foreach (var subscription in subscriptions) 
      subscription.Dispose();
  }

  internal async Task StartMachine()
  {
    var result = await client.StartProduction();
    if (result.IsError)
      Console.WriteLine($"could not start production: {result.ErrorMessage}");
  }

  internal async Task PrintMachineState()
  {
    var result = await client.GetState();
    if (result.IsError)
      Console.WriteLine($"could not retrieve machine state: {result.ErrorMessage}");
    else
      Console.WriteLine(result.Data);
  }

  internal async Task TriggerCarvingPatternChange(CarvingPattern pattern)
  {
    var result = await client.ChangeCarvingPattern(pattern);
    if (result.IsError)
    {
      Console.WriteLine($"could not trigger carving pattern change: {result.ErrorMessage}");
      return;
    }

    var handle = result.Data;
    handle.SubscribeToStatusChangedEvents(@event =>
    {
      Console.WriteLine($"carving pattern change {handle.ProcessId} status: {@event.NewStatus}");
      switch (@event)
      {
        case IConfirmStationEmptyRequestHandle confirmStationEmptyRequest:
          HandleConfirmStationEmptyRequest(confirmStationEmptyRequest);
          break;
        case ICarvingHeadChangeRequestHandle carvingHeadChangeRequest:
          HandleCarvingHeadChangeRequest(carvingHeadChangeRequest);
          break;
      }
    });
  }

  internal async Task<Result> ChangeCarvingPattern(CarvingPattern pattern)
  {
    var result = await client.ChangeCarvingPattern(pattern);
    if (result.IsError)
      return $"could not trigger carving pattern change: {result.ErrorMessage}";

    var handle = result.Data;

    await foreach (var @event in handle.EventStream)
    {
      Console.WriteLine($"carving pattern change {handle.ProcessId} status: {@event.NewStatus}");
      switch (@event)
      {
        case IConfirmStationEmptyRequestHandle confirmStationEmptyRequest:
          HandleConfirmStationEmptyRequest(confirmStationEmptyRequest);
          break;
        case ICarvingHeadChangeRequestHandle carvingHeadChangeRequest:
          HandleCarvingHeadChangeRequest(carvingHeadChangeRequest);
          break;
        case { NewStatus: ChangeCarvingPatternProcessStatus.Finished }:
          return Result.Success;
        case { NewStatus: ChangeCarvingPatternProcessStatus.Failed }:
          return Result.Error("carving pattern change failed");
      }
    }
    return Result.Error("process ended unexpectedly");
  }

  private async void HandleQualityCheckInterception(IQualityCheckInterceptionProcessHandle handle)
  {
    var result = await handle.Answer(CheckQuality(handle.Quality));
    if (result.IsError)
    {
      // log error message
      // maybe retry
    }
  }

  private void HandleConfirmStationEmptyRequest(IConfirmStationEmptyRequestHandle handle)
  {Console.WriteLine("Is Station Empty?");
    // get input
    handle.Confirm();
  }

  private void HandleCarvingHeadChangeRequest(ICarvingHeadChangeRequestHandle handle)
  {
    Console.WriteLine($"need carving head {handle.RequestedCarvingHead}");
    // get input
    handle.Confirm();
  }
}