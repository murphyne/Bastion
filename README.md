# Bastion&hairsp;<sup>β</sup>

Finite State Machine implementation for Unity.


## Entities

- `State` is responsible for making decision on next state transitions.
  It also implements state behaviour.

  However, it is recommended to delegate responsibility of `State` to the list
  of `Actions`. This will allow the combination of `Actions` to be changed
  later from the inspector window.

- `Action` is purposed to represent one aspect of the state.

- `Agent` is a State Machine implementation, it stores and changes current state.

- `Context` provides access to fields and properties, required for decision making.

Usage example can be seen in the `SampleScene.unity`.
