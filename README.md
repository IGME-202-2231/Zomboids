# Project Owen Beck

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Owen Beck
-   Section: 3

## Simulation Design

My simulation will create a map filled with people wandering freely. When the player clicks, a zombie is spawned at the location of the click.
The humans will flee as the zombie seeks out the nearest. On collision, the human will turn into a zombie and then seek with the other zombie.
This continues until every human is a zombie.

### Controls
Player can click to spawn zombies.
    -This is done with the mouse
    -Spawning zombies more frequently will cause the game to end quicker.

## Human Idle

This agent is just a human wandering slowly with an idle sprite attached.

#### State Transistions

- Humans all start in this state

## Human Fleeing
This agent is a human fleeing from a zombie, has a scared sprite attached.

#### State Transistions

- Humans turn into fleers when a zombie is introduced into the scene

# Zombie Transformation
This agent is a human turning into a zombie. Pauses as it transforms. Has a half zombie sprite attached.

#### State Transistions
- Humans transform on collision with a zombie

## Zombie Seeker
This agent is a zombie that seeks nearest human fleer.

#### State Transistions
- Humans turn into zombie after the transform timer ends

### State 1: Human Idle

**Objective:** Idle humans just wander carelessly.

### State 2: Human Fleer

**Objective:** Fleer Humans simply flee from zombies and transforming zombies

### State 3: Zombie Transformer

**Objective:** Has a timer and stands still while transforming into a zombie

### State 4: Zombie

**Objective:** Seeks out nearest human

#### Steering Behaviors

- _List all behaviors used by this state_
   - _If behavior has input data list it here_
   - _eg, Flee - nearest Agent2_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
   
## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_

## Make it Your Own

-I am making all my own assets including all the sprites
-I am introducing the clicking mechanic

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

