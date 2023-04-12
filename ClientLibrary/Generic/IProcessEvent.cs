namespace ClientLibrary;

// Note: This is an empty marker interface and I'd prefer a different solution.
public interface IProcessEvent { }

// A generic decision request event.
public interface IDecisionRequest<T> : IProcessEvent
{
	public bool HasBeenAnswered { get; }

	// The answer value. This can be used at a later point in time if a client tries to recreate what happened in the
	// process.
	// TODO: Should this throw if the request has not been answered?
	// It might be the only way to distinguish an answered null value from an invalid null value. We could also make T
	// notnull. I could not find anything speaking against it.
	public T? AnswerValue { get; }

	public Task<Result> Answer(T answer);
}