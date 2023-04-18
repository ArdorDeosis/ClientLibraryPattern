using System.Reactive.Linq;

namespace ReactiveTest;

internal sealed class DistinctUntilChangedFilterTest
{
  
public static void Run()
{
  var stateProvider = new StateProvider();
  var subscriptions = new List<IDisposable>
  {
    stateProvider.StateStream.DistinctUntilChanged().Subscribe(_ => Console.WriteLine("State Changed")),
    
    stateProvider.StateStream.DistinctUntilChanged(state => state.TopLevelValue)
      .Subscribe(_ => Console.WriteLine("Top Level Value Changed")),
    stateProvider.StateStream.DistinctUntilChanged(state => state.SubState)
      .Subscribe(_ => Console.WriteLine("SubState Changed")),
    stateProvider.StateStream.DistinctUntilChanged(state => state.SubState.Value).Select(state => state.TopLevelValue == 3)
      .Subscribe(_ => Console.WriteLine("SubState Value Changed")),
  };

  Console.WriteLine("pushing first state");
  var state = new State
  {
    TopLevelValue = 1,
    SubState = new SubState
    {
      Value = 1,
    },
    OptionalSubState = new SubState
    {
      Value = 1,
    },
  };

  stateProvider.PushState(state);
  stateProvider.PushState(state);
  state.TopLevelValue = 2;
  stateProvider.PushState(state);
  state.SubState = new SubState
  {
    Value = 3,
  };
  stateProvider.PushState(state);
  state.SubState = new SubState
  {
    Value = 3,
  };
  stateProvider.PushState(state);
  state.SubState = new SubState
  {
    Value = 4,
  };
  stateProvider.PushState(state);


  subscriptions.ForEach(subscription => subscription.Dispose());
}
}