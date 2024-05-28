# Game Programming Project: Alpha Build
#### Trello Link: https://trello.com/b/6e9WcBwF/cashmoneydash 
#### Github Link: https://github.com/JackHanni/CashMoneyDash 
#### Alpha Build: [unity link](https://play.unity.com/mg/other/alphabuild-2)


## Description 
– A description of each team member’s contribution to the build.
* Jack: Movements of main character (via Finite State Machine) and enemy Slime, Camera Settings, Coins and Gems, Level 1 design, Moving Platforms, camera, UI
* Ruimin: MainScreen, Mouse Movement(deprecated), Character Stats(ScriptableObject and Monobehavior), Singleton for all Managers, Portals, Level 2 design, Occlusion, UI 

## Details 
– Details regarding any changes made to your core gameplay mechanics based on playtest feedback.

* The camera aiming has been slowed down a bit and the auto-centering turned off to make it easier to control.

* Fixed the way the character moved along with the platform to avoid weirdness.

* Need to fix some transition bugs from jumping to grounding.

* Instead of ensuring only one character exists (done by generating a prefab and load in data during transitioning), I decided to put a prefab at the portal and ask the portal just change scene.

* The tutorial I found for UI does not suit our goal, so I will design on my own or look for other ones that is closer to Mario 64 (Ruimin).


## References
- Terrain, Portal, Occlusion: [Unity - 3drpg-core](https://learn.u3d.cn/tutorial/3drpg-core)
- Coin Effects: [Youtube - Unity 3D Platformer](https://www.youtube.com/playlist?list=PLiyfvmtjWC_V_H-VMGGAZi7n5E0gyhc37)
- Respawn, Checkpoint: [Youtube - 3D Platformer in Unity](https://www.youtube.com/watch?v=MxEgXOWBNFA&list=PLkp23zg4CAMV8fcsUYX1EwihX7WXhFH6N&index=20)
- Circular Health Bar: [Youtube - Sam Schiffer](https://www.youtube.com/watch?v=V5h2ClMUguQ)
