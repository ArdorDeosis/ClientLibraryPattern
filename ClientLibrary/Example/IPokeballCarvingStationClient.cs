namespace ClientLibrary;

public interface IPokeballCarvingStationClient
{
  /// <summary>
  /// Starts the pokeball production.
  /// </summary>
  Task<Result> StartProduction();

  /// <summary>
  /// Gets the machine's ID.
  /// </summary>
  Task<Result<Guid>> GetMachineId();

  /// <summary>
  /// Triggers a process to stop production.
  /// </summary>
  Task<Result<IStopProductionProcessHandle>> StopProduction();

  // OPTION 1
  
  /// <summary>
  /// Retrieves the state from the machine. 
  /// </summary>
  Task<Result<PokeballCarvingStationProductionState>> GetState();
  
  /// <summary>
  /// Registers for state updates.
  /// </summary>
  Task<Result<IRegistration<PokeballCarvingStationProductionState>>> RegisterForStateStream();
  
  // OPTION 2
  
  /// <summary>
  /// The current machine state, if available.
  /// </summary>
  PokeballCarvingStationProductionState? State { get; }
  
  /// <summary>
  /// The current machine state as observable.
  /// </summary>
  IObservable<PokeballCarvingStationProductionState> ObservableState { get; }

  /// <summary>
  /// Registers for quality check interception.
  /// </summary>
  Task<Result<IRegistration<IQualityCheckInterceptionProcessHandle>>> RegisterQualityCheckInterceptor();
}