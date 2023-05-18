# Capy Game

## Music_Manager Test TODOS:

### Core Functionality

1. **Sequential Play Test:** Verify that when two songs are triggered sequentially, the second song starts playing after the first one ends. X

2. **Transition Effect Test:** Ensure that songs with the transitionPlay property set to true transition from volume 0 to the desired volume over the specified transitionDuration. If the transitionDuration is greater then the the remaining time on the song, the song will loop in order to transition down for desired amount of time. X

3. **Sequential Transition Test:** When two songs are triggered sequentially and the second song has the transitionPlay property set to true, ensure that the second song transitions in after the first one. Also, the second song should transition out at the end. The first song should play normally without any volume transitions if transitionPlay is not set to true. X

4. **Single Song Transition Test:** When a song has the transitionPlay property set to true, it should transition from volume 0 to the desired volume at the beginning. As the song nears the end, it should transition from the current volume to 0. X

5. **Force Play Test:** When forcePlay is enabled, the currently playing song should be placed next in the queue. The new song should start playing immediately. X

6. **Force and Transition Play Test:** When both forcePlay and transitionPlay are enabled, the currently playing song should smoothly transition to volume 0 over the duration of transitionDuration The old song should also be moved to the front of the queue. The new song that gets played should transition from volume 0 to the desired volume. X

### Extra functionality

1. **Pause Test:** When the pause method is called, the currently playing music should transition to volume 0 if pause Transition is set to true. The music should then be paused via the audio source.
2. **Stop Test:** When the stop method is called, the currently playing music should transition to volume 0 if pause Transition is set to true. The music should then be stopped via the audio source.
3. **Resume Test:** When resume method is called, audio source.play is called and the audio will transition to correct volume when pause transition is true
4. **Standalone Clear Queue Test:** Clears the queue and leaves the currently playing music alone. Music that is on loop may have already began transitioning out before this method is called. This method will not halt that transition
5. **Clear Queue in Settings Test 1:** When play music is called, and clear que is called and also force play, the current song will transition or not transition to the newly passed in song. The current song is not added to beginning of queue and existing queue will be removed.
6. **Clear Queue in Settings Test 2:** When play music is called and clearQueue is true and force play is not true, the same thing as test 1 will happen but there will be a warning log explaining what has happened just in case the result is not clear to the user.

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
