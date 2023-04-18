namespace ClientLibrary;

public interface IPokeballCarvingStationProxy
{
  /// <summary>
  /// Starts the pokeball production.
  /// </summary>
  Task<Result> StartProduction(CancellationToken cancellationToken = default);
  Task<bool> TryStartProduction(CancellationToken cancellationToken = default);
  Task StartProductionOrThrow(CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets some machine data.
  /// </summary>
  Task<Result<Data>> GetMachineData();
  Task<Data?> GetMachineDataOrNull();
  Task<Guid> GetMachineDataOrThrow();

  /// <summary>
  /// Triggers a process to stop production.
  /// </summary>
  Task<Result> StopProduction(StopProductionProcessHandlerBase handler);
  Task<Result> StopProduction(OccupiedChamberResolutionBehaviour behaviour);
  Task<Result> StopProduction(Func<Data, OccupiedChamberResolutionBehaviour> decisionMethod);
  Task<Result> StopProduction(Func<Data, Task<OccupiedChamberResolutionBehaviour>> asyncDecisionMethod);

  
}