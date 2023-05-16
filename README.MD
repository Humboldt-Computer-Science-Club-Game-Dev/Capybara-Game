# Capy Game

## Music_Manager Test TODOS:

1. **Sequential Play Test:** Verify that when two songs are triggered sequentially, the second song starts playing after the first one ends.

2. **Transition Effect Test:** Ensure that songs with the transitionPlay property set to true transition from volume 0 to the desired volume over the specified transitionDuration. Moreover, when there's a duration equal to or less than transitionDuration left in the song, the song should smoothly transition to volume 0.

3. **Sequential Transition Test:** When two songs are triggered sequentially and the second song has the transitionPlay property set to true, ensure that the second song transitions in after the first one. Also, the second song should transition out at the end. The first song should play normally without any volume transitions if transitionPlay is not set to true.

4. **Single Song Transition Test:** When a song has the transitionPlay property set to true, it should transition from volume 0 to the desired volume at the beginning. As the song nears the end, it should transition from the current volume to 0.

5. **Force Play Test:** When forcePlay is enabled, the currently playing song should be placed next in the queue. The new song should start playing immediately.

6. **Force and Transition Play Test:** When both forcePlay and transitionPlay are enabled, the currently playing song should smoothly transition to volume 0 over the duration of transitionDuration or the remaining duration of the song, whichever is shorter. The song should also be moved to the front of the queue. The new song that gets played should transition from volume 0 to the desired volume.

## Wave System TODOS:

1. Spawn enemy wave from game object children. X
2. Check if all enemy's are dead. X
3. Spawn next wave. X
4. Play next level when all waves have been spawned.

## MVP TODOS:

1. Add player shooting. X
2. Make enemy take damage from player bullets. X
3. Add enemy shooting. X
4. Make player take damage from enemy bullets. X
5. Make it so that enemies can die. X
6. Make enemies stop scrolling when they get to there designated spot. X
7. Make enemies oscillate. X
8. Make it so that health packs move to left after they spawn. X

## Clean up TODOs:

1. Bullet.cs X
2. CharacterController2D.cs X
3. Enemy.cs X
4. Enemy_Death_Anim.cs X
5. Enemy_Gunner_Brain.cs X
6. Enemy_Movement_Brain.cs X
7. Enemy_Rest_Bound.cs X
8. Enemy_Roaming_Brain.cs X
9. ENV_Scroller.cs X
10. Event_System.cs X
11. Gun.cs X
12. Health.cs X
13. Health_Pack.cs X
14. Health_UI_Player.cs X
15. player.cs X
16. Player_Death_Anim.cs X
17. Player_UI_Death.cs X
18. Random_Number_Generator.cs X
19. Set_needed_active.cs X
20. Simple_editor_gizmo.cs X
21. Wave_System.cs X
