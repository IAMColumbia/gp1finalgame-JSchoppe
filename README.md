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
## Not Implemented
The archetype I am targeting has many complex systems. Due to the time constraints of this project some of
the key elements that persist in the archetype will not be implemented in the concept:
 - Creating units and expenditure of currency
 - Capturing factories and cities
 - Largely varied unit types
 - Ranged units
## Initial Concept Plan
This plan details the goals to be achieved over a four week cycle.
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


