# UniState - FSM for Unity
## Introduction
UniState is a minimal Finite State Machine solution inspired heavily by [the Stateless library](https://github.com/dotnet-state-machine/stateless), designed specifically to integrate within Unity's package manager.
### Why UniState
- Minimal and standard interface for communication
- Define very specific transitions using EntryFrom and ExitTo
- Extremely safe and optimised thanks to [UniHelper](https://github.com/3rd-Party-Guy/UniHelper)
- Integrates well with Unity's package manager, allowing you to write it off as a dependency or import it without NuGet for Unity
## Dependencies
UniState is extremely small, consisting of two source files and a very small dependency: [UniHelper](https://github.com/3rd-Party-Guy/UniHelper)
This makes UniState extremely easy to get started with, understand, and expand.
## Example
```csharp
using UniState;
using UnityEngine;

public class MathMover : MonoBehaviour
{
  // define your states and triggers
  public enum State { Diagonal, Circle };
  public enum Trigger { ToDiagonal, ToCircle };

  // declare your state machine
  StateMachine<State, Trigger> fsm;

  void Start()
  {
    // create FSM with initial state as Horizontal
    fsm = new StateMachine<State, Trigger>(State.Diagonal);

    // configure
    fsm.Configure(State.Diagonal)
      .Permit(Trigger.ToCircle, State.Circle)                        // define a transition
      .OnEntryFrom(State.Circle, () => Debug.Log("Woah, specific"))  // specific transition event
      .Ignore(Trigger.ToDiagonal);                                   // ignore a trigger

    fsm.Configure(State.Circle)
      .Permit(Trigger.ToDiagonal, State.Diagonal)
      .OnExitTo(State.Diagonal, () => Debug.Log("Isn't this specific enough for you?"))
      .Ignore(Trigger.ToCircle);
  }

  void Update() {
    if (fsm.State is State.Diagonal) {
      // diagonal logic here
    }
    else if (fsm.State is State.Circle) {
      // circle  logic here
    }
  }
}
```
