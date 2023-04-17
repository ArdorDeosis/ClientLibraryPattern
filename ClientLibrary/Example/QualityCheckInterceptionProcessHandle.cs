namespace ClientLibrary;

public interface IQualityCheckInterceptionProcessHandle : IProcessHandle, IDecisionRequest<QualityCheckApproval>
{
	QualityScanData Quality { get; }
}

public enum QualityCheckApproval
{
	Approved,
	Denied,
}