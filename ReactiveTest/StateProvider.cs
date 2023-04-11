using System.Reactive.Subjects;

namespace ReactiveTest;

public interface IStateProvider
{
  IObservable<State> StateStream { get; }
}

public class StateProvider : IStateProvider
{
  private readonly ReplaySubject<State> stream;
  public IObservable<State> StateStream => stream;

  public StateProvider()
  {
    stream = new ReplaySubject<State>(1);
  }

  public void PushState(State state)
  {
    Console.WriteLine("-------------------");
    stream.OnNext(state);
  }
}

public struct State
{
  public required int TopLevelValue { get; set; }
  public required SubState SubState { get; set; }
  public required SubState? OptionalSubState { get; init; }
}

public struct SubState
{
  public required int TopLevelValue { get; set; }

  void dhsajk()
  {
    var array = new int[5];
    var x = new ValueList<int>();
    array.AsValueList();
  }
}
public class  Wrapper
{
  public required int Value { get; set; }
}
