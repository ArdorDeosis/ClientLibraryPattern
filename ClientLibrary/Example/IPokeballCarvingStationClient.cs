namespace ClientLibrary;

public interface IPokeballCarvingStationClient
{
  Task<Result> StartProduction();
  Task<Result> StopProduction();

  Task<Result<PokeballCarvingStationProductionState>> GetState();
  IDisposable SubscribeToStateChanged(Action<PokeballCarvingStationProductionState> eventHandler);
  
  IDisposable SubscribeToPokeballFinished(Action<PokeballProductionResult> eventHandler);
  
  Task<Result<IChangeCarvingPatternProcessHandle>> ChangeCarvingPattern(CarvingPattern pattern);
  
  IDisposable RegisterQualityCheckInterceptor(Action<IQualityCheckInterceptionProcessHandle> processHandler);
}