CS 4455 Video Game Design Final Project

Team Members: 

Sophia Handel  
Alex Yang      
Bennett Hillier 
Kenji Tanaka    
Kristen Goldie

=======================================================================================================================
How to play:


Steer the character using the arrow keys, and jump/double-jump with the space bar.

Blue "shield" gems found throughout the level provide the player with protection from one hit.

Red heart gems add an additional health point to the player if their health is less than 3.

The player will need to hit the red button to open the door midway through the level to move through the rest of level.

The goal is to reach the end of the countertop, past the large mouse.

The game can be paused and unpaused with the ESC key.  


1. The tomato player displays squish and stretch expressive animation.

2. The mice, visible on the middle platforming part of the level, as well as the large mouse at the end of the level display player-chasing AI based on line of sight. The mice have multiple states including patrol, idle, and chase.

=======================================================================================================================of 
How Our Game Meets The Rubric:

3D Game Feel Game:
-Visual arrows to help guide player navigation
-Visible checkpoints and sounds to indicate the player has reached the checkpoint
-Sounds:
  -Player movement and death sounds
  -Sounds for AI when they hit the player, die, or call for help
  -Sizzle sound when the players is on a stove
  -Celebratory sound when player reaches a checkpoint or WINS the game
-Can die in various ways (stove, knives, AI, fall to death)
-Have a death counter and timer to keep track of player deaths and time playing the game
-Player respawns at most recent checkpoint on death if they choose “Respawn” option
-Player wins the game by collecting the final gem after the boss
-Player can look around with the mechanical mouse (not the AI mouse)

Precursors to Fun Gameplay:
-Player can complete the level in different ways:
  -Can try to get across the stoves or do the platforming
  -Can try to run away from the mice or kill them
-Completing the level in different ways causes different results:
  -Crossing the stove rather than platforming can result in damaging player health or death
  -May die if you fight the mice, but may run into an incredibly difficult situation later if you decide not to fight the mice
-The AI shows character and interaction:
  -The lookout mouse displays that the AI can communicate with each other
  -The mice by the cheese on the stool in the first half of the game give the AI some character (it’s also funny to watch)
-The player can interact with objects throughout the game (a button, checkpoints, collect hearts and shield)
-Player is rewarded when completing sections of the level and reaching a checkpoint (respawn at that point after death)
-The game progresses in difficulty throughout:
  -Start with basic platforming to get used to the controls, then you will encounter more advanced platforming
  -First falling knife field is less difficult than the second
  -Mice encounters get more difficult as you progress
-Aimed for obstacles to be challenging, but not so difficult to the point where it can make the game unenjoyable

3D Character with Real Time Control:
-Character control is essential for gameplay
-There is no delay between a button being pressed/mouse being moved and the player reacting
-Animations for walking, jumping, and turning make the character feel more alive
-Players can choose between using a keyboard or controller to move the character around
-Player can control the direction of fall, makes the game feel more responsive
-Player can double jump, allowing it to clear obstacles, and feel in control of the character while in the air

3D World with Physics and Spatial Simulation:
-Synthesized environment tailored to our character and our challenges
  -Stacked bowls, plates, and other kitchenware to create platforms
  -Used granite countertop shelves as platforms
  -Used sinks/stoves as large impassable regions
  -Used sponges as jump pads
  -Created walls to divide the countertop into sections
  -All of these were chosen to highlight the player’s jumping ability and to guide the player through the level
-Graphics align with physics representations
-Appropriate boundaries
  -Walls
  -Stoves (you die when you stay on them too long, and they are too big to run over)
  -Kitchen counter edge (if you fall off, you fall to the floor and die)
  -Kitchen sinks (if you fall in you die)
-Variety of environmental physical interactions
  -Buttons that you jump on to open giant doors
  -Jump pads to help you make longer jumps
  -Knives that land on the countertop and fall over, physically interacting with each other (2nd knife section, after the 5th checkpoint, right before the boss)
  -Animated floating plates so it is harder to jump on (after 4th checkpoint)
  -Arrows directing the player rotate for better visibility
  -A button animates so it looks pressed when you jump on it. Its color also changes
-Consistent spatial simulation

Real Time NPC Steering Behaviors/Artificial Intelligence:
-Created original animation controllers for mice to control idling/running, attacking, calling for help, and dramatically dying
-Multiple states of behavior for each type of mouse:
  -mice/guard mice/boss mouse: idle, patrol, look around for player, chase the player, attack the player, death
  -Look out mice: idle, patrol, look around for player, call for help, watch guard mice attack player, death
  -You can see this by getting close to a mouse. It will “see” you, and either start chasing you or call for help depending on what type of mouse it is. When you jump on top of mice, they die and play a death animation
-Smooth steering: you can see this if you run away from the mice, and they attempt to follow you
-Believable AI: Mice are simple creatures, like our characters. The mice do not like outsiders. They can only see the player when the player is in front of them, within a certain distance, and when there are no other objects in the way. The mice with weapons attack, and they continue to do so until they die or they can no longer see the player. They go to the last spot they saw the player, and if they still cannot see you, then they give up and go back to what they were doing before (idling or patroling). Some mice do not have weapons, and therefore need to call for help. If the help dies, then they call again
  -There are mice with weapons at the second, third, and fourth checkpoints. The mouse without weapons is after the third checkpoint
-There is reasonably fluid animation. We used a blend tree for the idle/walk/run, and a separate layer for upper body movement.
-Sensory feedback: the mice swing their stick when they are attacking you. There is a thump sound when they hit you. There is also a sound when you jump on top of them and kill them, as well as a dramatic death animation. There is also an animation and sound for when a mouse calls for help. You can usually tell when a mouse sees you because they start running at you, but they don’t make sounds because mice are sneaky
-Fairness: most mice can only see a 60 degree cone in front of them, and only within a certain radius. They cannot see through objects. The “call for help” mouse has a wider sight angle because it is more scared without weapons and looks around more.

Polish:
-We have a start menu, which you can see if you start with MainMenu Scene
-We have an in game pause menu with ability to quit the game(press esc)
-No debug output is visible
-Hot keys used for testing are disabled
-GUI elements are styled consistently (start menu, pause menu, death menu, you win menu, display in upper right corner during gameplay)
-We have water in the kitchen sinks that shifts as you move around so it does not look static
-The checkpoints glow
-There is a slight particle effect when you respawn
-There is auditory responses when you take damage by mice or stoves, when you deal damage to a mouse, when you walk, when you reach a checkpoint, when you click buttons
-Shading and lighting is consistent with theme
-Unified color palette

=======================================================================================================================
Known Bugs:

-Player can occasionally get pushed through the ground after being hit by an enemy, when falling from a high distance, or when under some obstacles.
-After killing a mouse enemy, it’s weapon can still add forces to the player (not damage).
-Camera can go through walls.
-Sometimes in the beginning, a plate can damage the player.
-If the player continuously jumps before touching the ground on spawn, the player can fly around the entire level.

=======================================================================================================================
External Resources:

Bob the Tomato model: https://sketchfab.com/3d-models/bob-the-tomato-model-ab4664db1e0e457aad11ed568807e078
Mice model: https://assetstore.unity.com/packages/3d/characters/animals/mouse-knight-pbr-polyart-135227
Tomato squish sound: https://freesound.org/people/HonorHunter/sounds/271666/
Thump sound when mice hits player: https://freesound.org/people/jameswrowles/sounds/380640/
Sound when mice die: https://freesound.org/people/hermengarde/sounds/235845/
Sound when mouse calls for help: https://freesound.org/people/AntumDeluge/sounds/188043/
Stove sound: http://soundbible.com/1090-Hot-Sizzling.html
Checkpoint sound: https://freesound.org/people/Vicces1212/sounds/123751/
Collectable: https://assetstore.unity.com/packages/3d/props/simple-gems-ultimate-animated-customizable-pack-73764
Arrow and Checkpoints: https://assetstore.unity.com/packages/tools/particles-effects/arrow-waypointer-22642
Kitchen Accessory: https://assetstore.unity.com/packages/3d/props/interior/kitchen-props-free-80208
Knives: https://assetstore.unity.com/packages/3d/props/weapons/combat-knives-4429
Kitchen Creation Kit:
https://assetstore.unity.com/packages/3d/environments/kitchen-creation-kit-2854
Water Shader: https://assetstore.unity.com/packages/vfx/shaders/mobile-depth-water-shader-89541
Sponge Texture: https://assetstore.unity.com/packages/2d/textures-materials/water/foam-textures-72313

==========================================================================================

Sophia: Level Engineer / UI
Alex: Level Designer 
Bennett: Level Engineer
Kenji: Character Designer/Engineer
Kristen: AI Designer/Engineer





