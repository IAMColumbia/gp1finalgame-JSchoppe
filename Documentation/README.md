# Vertical Slice - Design Choices
<details>
<summary>This section documents the design choices made for the vertical slice build.</summary>

## New Unit: Tank
In this update I added a second unit, the tank, to test the scalability of the damage table and unit
interactions. The tank is much more powerful than the foot soldier, and is effectively OP without a third
unit to balance out a weapon triangle.
## Terrain Implementation
Terrain has been implemented using a processor that checks a unity tilemap. This approach was chosen
since instantiating gameobjects on every tile performs poorly. This choice increases Unity dependency
and more research needs to be done about whether Unity exposes lower level APIs for sprite rendering.
<br>
Units now respond to terrain; if a commander mouses over a non-traversable tile, the corresponding
unit will attempt to use A* pathfinding to infer a path around the obstacle (or get as close as they can).
The introduction of A* introduces some strange continuity bugs with the pathing that need to be addressed.
## AI Improvement
The AI model has been updated to react to the target location of the enemy paths. It will try to meet enemy
units at their destination. Coincidentally this has made the AI very strong at last second moves. The AI
additionally incorporate the damage table to peek interactions and look for favorable engagements; although
notably it does not calculate every possible engagement along its move path.
<br>
A bug has been fixed where the AI would interrupt its own actions. The AI will not reconsider its moves
until it has finished all previously pushed actions. This strategy may not be scalable due to long pauses
between reconsideration (in the future may want the AI to think between every action instead of in batches).
## Known Code Issues
There are some edge cases that cause the new unity input system to log errors but it is unclear what causes this.
<br>
MonoBehaviour dependence has increased in this update. More items need to be converted to designer 
instances to decouple from the Unity engine. This has resulted in a handful of hotfixes that subvert
the underlying core code structure.
<br>
Some scripts are sorely lacking refactoring, comments, and XML documentation.
## Known Gameplay Issues
The last second moves from the AI are frustrating. The small map scale magnifies the last second
chaos of the gameplay style. Larger maps should be tested to see how the dynamic changes.
<br>
There is no win/lose condition reaction.
<br>
Cursor state is not properly reset between rounds.

</details>

# Proof of Concept - Design Choices
<details>
<summary>This section documents the design choices made for the proof of concept build.</summary>

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

</details>
