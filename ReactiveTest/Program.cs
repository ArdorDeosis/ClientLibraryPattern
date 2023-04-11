// See https://aka.ms/new-console-template for more information

using System.Reactive.Linq;
using ReactiveTest;

var stateProvider = new StateProvider();
var subscriptions = new List<IDisposable>
{
  stateProvider.StateStream.DistinctUntilChanged().Subscribe(_ => Console.WriteLine("State Changed")),
  stateProvider.StateStream.DistinctUntilChanged(state => state.TopLevelValue).Subscribe(_ => Console.WriteLine("Top Level Value Changed")),
  stateProvider.StateStream.DistinctUntilChanged(state => state.SubState).Subscribe(_ => Console.WriteLine("SubState Changed")),
  stateProvider.StateStream.DistinctUntilChanged(state => state.SubState.TopLevelValue).Subscribe(_ => Console.WriteLine("SubState Value Changed")),
};

Console.WriteLine("pushing first state");
var state = new State
{
  TopLevelValue = 1,
  SubState = new SubState
  {
    TopLevelValue = 1,
  },
  OptionalSubState = new SubState
  {
    TopLevelValue = 1,
  },
};

stateProvider.PushState(state);
stateProvider.PushState(state);
state.TopLevelValue = 2;
stateProvider.PushState(state);
state.SubState = new SubState
{
  TopLevelValue = 3,
};
stateProvider.PushState(state);
state.SubState = new SubState
{
  TopLevelValue = 3,
};
stateProvider.PushState(state);
state.SubState = new SubState
{
  TopLevelValue = 4,
};
stateProvider.PushState(state);


subscriptions.ForEach(subscription => subscription.Dispose());