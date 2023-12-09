# Project Owen Beck

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Owen Beck
-   Section: 3

## Simulation Design

My simulation will create a map filled with people wandering freely. When the player right clicks, a zombie is spawned at the location of the click.
The humans will flee as the zombie seeks out the nearest. On collision, the human will turn into a zombie and then seek with the other zombie.
This continues until every human is a zombie. If player right clicks they shoot a bullet which will kill zombies and humans. If player clicks space, 1 or 2 trucks will spawn and kill vereyone in their path.

### Controls
Player can click to spawn zombies.
    -This is done with the mouse
    -Spawning zombies more frequently will cause the game to end quicker.

# Agents
## Human Idle

This agent is just a human wandering slowly with an idle sprite attached.

#### State Transistions

- Humans all start in this state

## Human Fleeing
This agent is a human fleeing from a zombie, has a scared sprite attached.

#### State Transistions

- Humans turn into fleers when a zombie is introduced into the scene

## Zombie Transformation
This agent is a human turning into a zombie. Pauses as it transforms. Has a half zombie sprite attached.

#### State Transistions
- Humans transform on collision with a zombie

## Zombie Seeker
This agent is a zombie that seeks nearest human fleer.

#### State Transistions
- Humans turn into zombie after the transform timer ends

## Blood
This agent is just a static pool of blood

#### State Transistions
-Any agent will turn into blood if shot or hit by truck

# States
### State 1: Human Idle

**Objective:** Idle humans just wander carelessly.

### State 2: Human Fleer

**Objective:** Fleer Humans simply flee from zombies and transforming zombies

### State 3: Zombie Transformer

**Objective:** Has a timer and stands still while transforming into a zombie

### State 4: Zombie

**Objective:** Seeks out nearest human

### State 5: Blood

**Objective:** Object is dead
   
   
## Sources

-   All Art assets are made by me
-   Splat SFX: https://www.zapsplat.com/music/8bit-splat-bomb-boom-blast-cannon-classic-cartoon/
-   Truck SFX: https://www.zapsplat.com/music/truck-semi-horn-beep-honk/

## Make it Your Own

-I am making all my own assets including all the sprites
-I am introducing the clicking mechanic for zombie spawning and sniper shooting
-I added all logic for Bus movement

## Known Issues

-Sometimes zombies can just stand still if theyre not near any humans.

### Requirements not completed

_If you did not complete a project requirement, notate that here_

