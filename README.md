# Game Programming Project: Beta Build
#### Trello Link: https://trello.com/b/6e9WcBwF/cashmoneydash 
#### Github Link: https://github.com/JackHanni/CashMoneyDash 
#### Beta Build: [unity link](https://play.unity.com/mg/other/webgl-builds-414377)


## Description 
– A description of each team member’s contribution to the build.
* Jack: Movements of main character (via Finite State Machine) and enemy Slime, Camera Settings, Coins and Gems, Level 1 design, Moving Platforms, camera, UI
* Ruimin: MainScreen, Mouse Movement(deprecated), Character Stats(ScriptableObject and Monobehavior), Singleton for all Managers, Portals, Level 2 design, Occlusion, UI 

## Details 
– Details regarding any changes made to your core gameplay mechanics based on playtest feedback.

* Enemies fully implemented with patrols, agro range, attack and damage for both enemy and player, death animation, coin spawn on death, and smoke poof (bugged in build).

* Respawn at a low height.

* Background music and sound effects on coin collect and gem collect.

* Portal transitions.

* Canvas displays health and number of coins and gems collected.

* Want to change movement to momentum-based movement and change controlled jump velocity to feel better.

* Enemies need to travel past the player rather than moving to them and waiting.

* Need to fix some transition bugs from falling to grounding.


## References
- Terrain, Portal, Occlusion: [Unity - 3drpg-core](https://learn.u3d.cn/tutorial/3drpg-core)
- Coin Effects: [Youtube - Unity 3D Platformer](https://www.youtube.com/playlist?list=PLiyfvmtjWC_V_H-VMGGAZi7n5E0gyhc37)
- Respawn, Checkpoint: [Youtube - 3D Platformer in Unity](https://www.youtube.com/watch?v=MxEgXOWBNFA&list=PLkp23zg4CAMV8fcsUYX1EwihX7WXhFH6N&index=20)
- Circular Health Bar: [Youtube - Sam Schiffer](https://www.youtube.com/watch?v=V5h2ClMUguQ)
- https://github.com/RYanXuDev/PlatformerControllerTutorial 
