namespace ClientLibrary;

public interface IQualityCheckHandler : IProcessHandler
{
	public Task<Result<bool>> CheckQuality(QualityScanData data);

}

public abstract class QualityCheckHandler
{
	public virtual Task<Result<bool>> CheckQuality(QualityScanData data) => 
		Task.FromResult(Result<bool>.Success(true));

	protected virtual void OnStuffHappened() { }

	public static QualityCheckHandler FromValue(bool value) => new ConstantValueQuality(value);
}

internal class ConstantValueQuality : QualityCheckHandler
{
	private readonly bool value;
	
	public ConstantValueQuality(bool value)
	{
		this.value = value;
	}

	public override Task<Result<bool>> CheckQuality(QualityScanData data) => 
		Task.FromResult<Result<bool>>(value);
}


class dummy
{
	void dostuff()
	{
		QualityCheckHandler.FromValue(true);
	}
}