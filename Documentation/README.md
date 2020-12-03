# Proof of Concept - Design Choices
This section documents the design choices made for the proof of concept build.
## Monobehaviour Avoidance
As an attempt to make the structure of this game as modular as possible, I have been slowly
implementing a pattern where I strip MonoBehaviour dependencies from underlying logic. Currently
I am achieving this by having "designer" scene entities that just contain constructor arguments
serialized to the inspector. The visual components that do require MonoBehaviour use event listeners
to respond to change in state inside the underlying core gameplay logic. This in effect has made
the gameplay logic deaf to the outside rendering logic; completely unaware of its existence. This
requires extensive use of events and I am still not entirely sure it is the best option.
## Update Loop Hijacking
Some of the underlying scripts that have stripped MonoBehaviour still need access to the update loop.
In the current implementation this is solved by hijacking the update loop of an instantiated singleton.
It has yet to be tested whether loading all of these functions onto one delegate in one monobehaviour
effects performance.
## TileGrid Dependency
Almost all of the underlying gameplay logic classes communicate by observing the state of the TileGrid
and the objects that exist in its collections. This in affect makes it relatively easy to observe state
but has some notable inefficiencies that pop up as well as posing long term encapsulation issues.
## Known Code Issues
Some of the scripts need refactoring and clean up. There are some hacks marked with TODOs that circumvent
structure and need to be addressed to prevent scaling and encapsulation issues.
## Known Gameplay Issues
The state machine for the cursor has some bugs that need to be fixed (edge cases). The one feature that
was not able to be implemented in time was the clearing of conflicts at the end of the combat phase. While
it can be easily implemented, it will create more clean up work since it is based on the normal blend function
which needs cleanup (so I've decided to wait for this build). AI has some timing issues; but meets the set out
goal for POC where it is able to interface with a cursor. Arrow paths can be hard to read since they can overlap,
better UX needs to be explored on this issue.
