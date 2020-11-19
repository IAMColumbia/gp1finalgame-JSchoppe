# Skirmish Wars
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

