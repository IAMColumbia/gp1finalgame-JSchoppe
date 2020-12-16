# Skirmish Wars
An experimental gameplay concept that combines turn based and real time strategy elements
in tile based unit combat.
<br>
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/thumbnail.jpg">
<br>
Skirmish Wars seeks to experiment with turn based formulas by questioning a fundamental
component of the Wars strategy gameplay. Instead of players taking turns moving units,
this game allows both players to command units at the same time and at the end of a turn
clock all units will move synchronously. The game explores the frantic gameplay styles
that are afforded by this change, and encourages more strategies based on psychology and game theory.
<br><br>
A big challenge with this game is balancing excitement with anxiety. Not being able to fully process
and make calculated moves is a purposefully designed aspect of this game. It is meant to be more
chaotic, while at the same time remaining rewarding to players who devise strategies quickly and
react to their opponents strategy.

## Contributors
<div align="center">
  <div style="display: flex;">
    <img src="https://avatars1.githubusercontent.com/u/38409262?s=460&u=f0e7f13731979e49c5f6240ee56c6556bb88a5fa&v=4" width="50px">
    <img src="https://avatars1.githubusercontent.com/u/1069059?s=460&u=d7795dacb9505f51922adc41b66e728f3fa54cd1&v=4" width="50px">
    <img src="https://avatars0.githubusercontent.com/u/44657886?s=460&u=1db6506050691e865b13678cedb08a260a4d4cff&v=4" width="50px">
    <img src="https://avatars0.githubusercontent.com/u/54965702?s=460&u=8ca37de9a002d3f39fa7cadb84fb76363b78ac6b&v=4" width="50px">
    <img src="https://avatars0.githubusercontent.com/u/51983062?s=460&u=9d932597693b910276f21a29d1bfcfc3a93541fc&v=4" width="50px">
  </div>
</div>

 - Programming
   - [Jethro Schoppenhorst](https://github.com/JSchoppe)
 - Feedback/Code Review
   - [Jeff Meyers](https://github.com/dsp56001)
   - [David Nichol](https://github.com/DavidJNichol)
   - [Brendan Lienau](https://github.com/Kobakat)
   - [Andy Ocampo](https://github.com/andyocampo)
 - Open Source Toolchain
   - [Paint.NET](https://github.com/paintdotnet)
 - Closed Source Toolchain
   - [Unity](https://github.com/Unity-Technologies)
   - [Visual Studio](https://visualstudio.microsoft.com)
   - [GitHub](https://github.com/github)

## Releases & Post-Mortems
<details>
<summary>Release 0.1</summary>
 
- scalable terrain with defense
- unit types with scalable damage table
- enemy combat cycle that accounts for unit type and terrain
- player and ai cursors, ability to peek the enemy movements
- basic ai that utilizes damage table
- prototype ui with unit counts, and controls
- pausing mechanism that blocks current gameplay

<details>
<summary>Postmortem</summary>

## Progress Overview
Many of the desired features were realized in this release. The primary lacking
feature from the proposal is a fully fleshed out combat phase. I realized that
during development that there are more edge cases with unit movement that would be
worth addressing (ie what happens when units are within a half unit during movement).
I did not foresee these complexities.

Overall, a decent slice of the codebase is Unity independent. Everything within the core
folder is meant to be, but the phases have yet to be generalized away from MonoBehaviour.
Reusability could be further improved by wrapping more components of the engine. Mathf and
Vectors are a prime example where creating a wrapper could greatly increase mobility.
## UML Analysis
### Combat Units
The combat units have a good foundation in being MonoBehaviour independent, they largely act as state
containers and it could be argued that the movement based methods should be abstracted away (ie `RefreshMoveOptions()`).
It is questionable whether the tile actor base class is neccasary. I placed this base class in place under the circumstance
than a non-unit tile entity might be added later. The `SpriteChainRenderer` is a class that admittedly contains
a lot of implementation that is engine specific when it doesn't need to be. Although a core class may not be appropriate,
some of the underlying logic would fit much better in a helper class. The same can be said about `TileIndicatorPool`.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/combat-unit-uml.jpg">
### Commanders
The commander classes I am pretty happy with, they are fairly easily extended without MonoBehaviour dependence.
The AI behavior is very primitive, this problem itself begs a lot of thought into how complex the AI thought should
be and how computationally expensive it would be under scaling map sizes.
As a side note; all cases of classes postfixed by `Instance` are constructors implemented using Unity scene instances.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/commander-uml.jpg">
#### Commander Panels
The commander panel is a UI element that was implemented very late in the project cycle. The improvements here are obvious;
abstract away from MonoBehaviour and implement their accessibility through `IDesignerParser`.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/commander-panel-uml.jpg">
### Cursors
While cursors are typically very simple, for this game I wanted player to be able to see each others moves during playtime.
This was achieved through the cursor controller and it's Unity renderer. I think this is maybe one of the best examples of engine
independency and how more of the project should be modeled. All of the cursor controller functionality, including visual state, is
on the core level. Unity only handles the movement of the sprite object and applying the changes in visual state. Although notably
the use of events is perhaps not the best solution, given that there is only one intended listener it may be better to use delegates
directly in this pattern.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/cursor-uml.jpg">
### Tile Grid
The grid serves largely as the main highway for actors in the game to observe other actors. Initially it was just meant to represent the
data of the grid. The fact that many classes depend on the grid poses some encapsulation problems. It may have been wiser to partition
the accessors and methods in some way, whether it be by a referenced instance or by partitioning interfaces. The gird also contains the
A* implementation, which should certainly be abstracted into a utility. Also shown here is the damage table, which is implemented using a
two dimensional accessor. This solution is efficient and fairly easy to scale, but is hard to effectively implement in the unity designer;
a spreadsheet should probably be used instead for this purpose.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/tile-grid-uml.jpg">
### Phases
Phases are largely dependent on Unity in their current implementation; although they very much could be abstracted away using
a similar technique to other classes using designer `Instance` classes. The phases could use a lot of work on the animation
front, as well as telegraphing to improve user experience. There is some attrocious last second code in here as well when it
comes to calculating damage on the unit clusters each step that needs further refactoring.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/phases-uml.jpg">
### Terrain
The tile terrain is implemented in such a way where an accessor is used to get terrain data for a specific unit type. This works
well and is scalable. Multiple tiles on a grid can point to the same terrain data and accessor.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/terrain-uml.jpg">
### Teams
Team association is one of the features in the game that still needs to be tested for scalability. The singleton should be abstracted
from monobehavior and perhaps should not be a static singleton (similar to how grid is not a static singleton).
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/teams-uml.jpg">
### Timing
The `Timer` class implemented here latches onto the unity engine update loop (some other classes do as well). The `UpdateContext` should
be defined using an interface so that it can be retargeted easily to other engines. The Timer itself is a very standard timer implementation
that triggers events.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/timer-uml.jpg">
### Design Parser
The `IDesignerParser` interface shown here is meant to request the utilized engine a means with which the game should process initialization
of a designed map. The exposed methods could use much refinement in how they access design elements; in the current build they are meant to
facilitate a very basic map. In the future this interface and its implementation should be much more fleshed out to minimize the amount
of engine specific code required to load more complex stages.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme%20Images/release-0.1/designer-parser-uml.jpg">

</details>

</details>

## Project Proposal
<details>
<summary>The initial proposal for the project</summary>
 
Genre: 2D Tile-based Stategy

Skirmish Wars is an experimental gameplay concept that seeks to challenge some
fundamental formula components in tile based combat games such as Advance Wars
and, more recently, WarGroove.

<img src="http://michibiku.com/wp-content/uploads/2016/05/superfamicomwars-800x445.jpg" width="30%">
<img src="https://cdn.gamer-network.net/2019/articles/2019-01-30-13-04/-1548853494549.jpg" width="30%">

## Experimentation
One of the noticeable shortcomings of the archetype is the exponential increase in play time as you get
into the late game with many units. This concept will attempt to solve this issue in a similar way that
Speed Chess seeks to solve the slow lategame of chess. This project will experiment with the formula in
the following ways (included are my hypotheses for how this will effect gameplay):
 - Fixed time limit for turns
   - Prevents late game fatigue when there are large unit counts
   - Makes it easier for lead trades due top harder time management for leading player
 - All players plan their turn concurrently and can see other player's plans
   - Strategies will be more reactive, rather than focused on long term setups
   - Will introduce more strategies based on psychology and game theory
 - All enemies move concurrently
   - Eliminates the turn order advantage
   - Prevents enemy range standoffs since there is not first strike advantage
   - Players will have to choose what to pay attention to during combat phase

The following diagram further demonstrates the key change in gameplay over time. The traditional formula
is shown on top with the proposed concurrent model below.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/turn-order.jpg" width="80%">

### Not Implemented
The archetype I am targeting has many complex systems. Due to the time constraints of this project some of
the key elements that persist in the archetype will not be implemented in the concept:
 - Creating units and expenditure of currency
 - Capturing factories and cities
 - Largely varied unit types
 - Ranged units
## Initial Concept Plan
This plan details the goals to be achieved over a four week cycle. The following UML diagrams show the
general program flow for the gameplay. Red regions show where un-implemented features would go tentatively.
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/top-level-loop.jpg" width="100%">
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/command-phase.jpg" width="100%">
<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/attack-phase.jpg" width="100%">
### Proof of Concept
The proof of concept will only feature the top-level gameplay cycle. It will implement:
 - General Framework
   - Tile Grid
   - Tile Pathing
   - Foot Soldier Enemy Type
 - Command Phase
   - Phase Timer (implement action interrupters)
   - Input Cursor (set unit paths)
   - AI Input Cursor (random unit behavior)
   - Spy ability (render AI cursor and movement choices)
 - Attack Phase
   - Concurrent Enemy Movement
   - Actor Skirmish (apply damage by comparing damage tables)
   - Movement Exhausted Skirmish (fight until defeat of one side)

### Vertical Slice
The vertical slice will feature more meaningful gameplay. It will implement:
 - General Framework
   - Terrain Types (sprite and movement cost differences)
     - Void (solid black, allows map to be non-rectangular)
     - Ocean (non-navigable)
     - Urban (battles yield less damage)
     - Plains (battles yield more damage)
   - Basic Designed Stage
   - Foot Solider Variation (different damage? movespeed?)
 - Command Phase
   - AI Input Improvements (uses damage table to look for favorable trades)
 - Attack Phase
   - Terrain Effects Skirmish Calculations
### Concept First Build
This stage will be used primarily to clean up the demo so that it can be
easily distributed to, and understood by, players of the genre. This will
allow data to be gathered to refute/confirm the concept hypotheses.
 - Formal Game Elements
   - Implement Pause Screen
   - Implement Victory/Defeat Screen (hides strategy content)
 - Player GUI
   - Unit Count
 - Button Indicators
   - Implement New Input System
 - Bug Fixes
## Initial Concept UML
The following UML model shows my initial thought process for tackling this concept.
### Cursor Implementation
This diagram shows how the player and competing AI will interface with the cursor.
The player cursor is directly controlled by the mouse, while the AI cursor is directly driven
by the agent using the action struct.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/uml-cursors.jpg">

### Phases Implementation
This diagram shows how the top level game loop is enforced in a generalized way using events.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/uml-phases.jpg">

### Commander Implementation
The commander implements the behavior for actually setting unit routes. Subclasses are used to drive the AI agent
and for the player to respond to toggling the spy mechanic and pausing.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/uml-commanders.jpg">

### Grid Implementation
The grid contains the data for tiles and the actors that can exist on those tiles. Grid actors can implement
special sprite behavior in response to how commanders interact with them.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/uml-grid.jpg">

### Damage Table Implementation
The damage table takes in designer specified pairs as a serialized struct and converts them to a table
that other classes can quickly access when doing damage calculations.

<img src="https://github.com/IAMColumbia/gp1finalgame-JSchoppe/blob/master/Documentation/Readme Images/uml-damage-table.jpg">

</details>
