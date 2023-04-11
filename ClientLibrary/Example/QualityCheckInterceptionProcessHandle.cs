namespace ClientLibrary;

public interface IQualityCheckInterceptionProcessHandle : IProcessHandle
{
  QualityScanData Quality { get; }
  
  ChangeCarvingPatternProcessStatus Status { get; }
  
  Task<Result> Answer(QualityCheckInterceptionProcessStatus status);
}

public enum QualityCheckInterceptionProcessStatus {
  WaitingForAnswer,
  Answered,
}
