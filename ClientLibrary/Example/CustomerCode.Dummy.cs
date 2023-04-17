namespace ClientLibrary;

internal partial class CustomerCode
{
  private void HandleMachineStateChanged(PokeballCarvingStationProductionState state)
  {
    // if machine is in error state, alert personnel
  }

  private void HandlePokeballFinished(PokeballProductionResult result)
  {
    // do something with the result, e.g. save it to a database
  }

  private QualityCheckApproval CheckQuality(QualityScanData handleQuality)
  {
    throw new NotImplementedException();
  }

  private void DoStuffWith(params object[] arguments)
  {
    throw new NotImplementedException();
  }
}