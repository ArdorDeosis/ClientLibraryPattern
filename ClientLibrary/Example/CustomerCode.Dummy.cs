namespace ClientLibrary;

internal partial class CustomerCode
{
  internal async Task StopMachine()
  {
    var result = await client.StopProduction();
    if (result.IsError)
      Console.WriteLine($"could not stop production: {result.ErrorMessage}");
  }

  private void HandleMachineStateChanged(PokeballCarvingStationProductionState state)
  {
    // if machine is in error state, alert personnel
  }

  private void HandlePokeballFinished(PokeballProductionResult result)
  {
    // do something with the result, e.g. save it to a database
  }

  private QualityCheckInterceptionProcessStatus CheckQuality(QualityScanData handleQuality)
  {
    throw new NotImplementedException();
  }
}