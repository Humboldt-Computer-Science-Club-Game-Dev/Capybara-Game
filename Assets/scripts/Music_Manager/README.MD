# Music_Manager Class Documentation

Music_Manager is a singleton class in Unity used to handle background music playback, including transitions and volume control. It works in conjunction with MusicSettings and MusicRequest structs.

## Class Usage

### Initialization

To initialize the Music_Manager, the class must be attached to a GameObject in your scene, preferably one dedicated to handling music. It requires an AudioSource component on the same GameObject.

### Playing Music

To play music, call the PlayMusic static method. Pass the name of the music file (without file extension) and a MusicSettings struct with your desired settings. The system will look for the music file in a "Resources/music/" directory in your project.

```csharp
MusicSettings settings = new MusicSettings();
settings.forcePlay = true;
settings.clearQueue = true;
settings.transitionPlay = true;
settings.pauseTransitions = false;
settings.transitionDuration = 1.0f;
settings.loop = true;
settings.volume = 1.0f;
settings.pitch = 1.0f;

Music_Manager.PlayMusic("MySong", settings);
```

### Resuming Music

To resume the currently paused music, call the ResumeMusic static method.

```csharp
Music_Manager.ResumeMusic();
```

### Pausing Music

To pause the currently playing music, call the PauseMusic static method.

```csharp
Music_Manager.PauseMusic();
```

### Stopping Music

To stop the currently playing music, call the StopMusic static method.

```csharp
Music_Manager.StopMusic();
```

### Clearing the Queue

To clear the music queue, call the ClearQueue static method.

```csharp
Music_Manager.ClearQueue();
```

### Structs Usage

#### MusicSettings

`MusicSettings` is a struct to set the various settings for the music track.

```csharp
public struct MusicSettings
{
public bool forcePlay;
public bool clearQueue;
public bool transitionPlay;
public bool pauseTransitions;
public float transitionDuration;
public bool loop;
public float volume;
public float pitch;
}
```

#### MusicRequest

`MusicRequest` is a struct which pairs an `AudioClip` and a `MusicSettings` struct together for handling by the `Music_Manager`.

```csharp
struct MusicRequest
{
public AudioClip musicClip;
public MusicSettings musicSettings;

    public MusicRequest(AudioClip clip, MusicSettings settings)
    {
        musicClip = clip;
        musicSettings = settings;
    }

}
```

### Important Notes

Ensure to have an `AudioSource` component attached to the same `GameObject` as the `Music_Manager`. The `Music_Manager` does not automatically create an AudioSource.

Music files must be placed inside a "Resources/music/" directory in your project for the `Music_Manager` to find and load them.

The `Music_Manager` supports volume transitions between different music tracks, enabling smooth transitions between music tracks.

The `Music_Manager` is a singleton and it should not be instantiated more than once per scene.

## Music_Manager Class - Sequential Play Feature

The `Music_Manager` class provides a feature to play music in sequence. This feature has been tested and confirmed to work as expected.

### Sequential Play Feature

The sequential play feature allows multiple music tracks to be queued and played one after another without any overlap. When a song finishes, the next song in the queue will start playing immediately.

In this scenario, music tracks are loaded and queued using the `PlayMusic` function, which accepts a `string` (the name of the music track) and a `MusicSettings` object. If the `forcePlay` property is set to `false`, the song will be added to the queue and will play in sequence when the current song finishes.

Here is an example of how to use the sequential play feature:

```csharp
void test1Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false; // Will not interrupt the currently playing music.
    musicSettings.transitionPlay = false; // Will not use transition effects when the music starts or stops.
    musicSettings.pauseTransitions = false; // Will not use transition effects when the music is paused or resumed.
    musicSettings.transitionDuration = 3f; // The duration of the transition effect, if used.
    musicSettings.loop = true; // The music will loop if there are no other tracks in the queue.
    musicSettings.volume = 1; // The volume level of the music.
    musicSettings.pitch = 1; // The pitch of the music.

    // Queueing music tracks
    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings); // 1st track to play
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings); // 2nd track to play
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings); // 3rd track to play
    Music_Manager.PlayMusic("cappy_defense_force_1", musicSettings); // 4th track to play
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings); // 5th track to play
}
```

In the above example, five music tracks are loaded and queued in the specified order. As `forcePlay` is set to `false`, the music tracks will play sequentially without interrupting the currently playing music track.

Please note that the `Music_Manager` class needs an instance in the scene to operate, so make sure to add and set up the `Music_Manager` object in your Unity scene. The `PlayMusic` function will issue a warning if it cannot find an instance of `Music_Manager` in the scene.

## Music_Manager Class - Transition Effect Feature

The `Music_Manager` class provides a transition effect feature for playing music. This feature allows a smooth volume transition when the music track starts playing. The feature has been tested and confirmed to work as expected.

### Transition Effect Feature

The transition effect feature works when the `transitionPlay` property of the `MusicSettings` object is set to `true`. The volume of the song starts from 0 and gradually transitions to the desired volume over a duration specified by the `transitionDuration` property.

When `transitionPlay` is set to `true`, if the `transitionDuration` is longer than the remaining duration of the song, the song will loop in order to complete the full transition effect.

Here is an example of how to use the transition effect feature:

```csharp
void test2Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false; // Will not interrupt the currently playing music.
    musicSettings.transitionPlay = true; // Will use transition effects when the music starts or stops.
    musicSettings.pauseTransitions = false; // Will not use transition effects when the music is paused or resumed.
    musicSettings.transitionDuration = 3f; // The duration of the transition effect.
    musicSettings.loop = true; // The music will loop if there are no other tracks in the queue or if the transition duration is longer than the song.
    musicSettings.volume = 1; // The target volume level of the music after transition.
    musicSettings.pitch = 1; // The pitch of the music.

    MusicSettings musicSettings2 = new MusicSettings();
    musicSettings2.forcePlay = false; // Will not interrupt the currently playing music.
    musicSettings2.transitionPlay = false; // Will not use transition effects when the music starts or stops.
    musicSettings2.pauseTransitions = false; // Will not use transition effects when the music is paused or resumed.
    musicSettings2.transitionDuration = 3f; // The duration of the transition effect.
    musicSettings2.loop = true; // The music will loop if there are no other tracks in the queue.
    musicSettings2.volume = 1; // The volume level of the music.
    musicSettings2.pitch = 1; // The pitch of the music.

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings); // Will use transition effects.
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings2); // Will not use transition effects.
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings); // Will use transition effects.
}
```

In the above example, three music tracks are loaded and queued. The first and third tracks will use the transition effect while the second track will not. This demonstrates how to utilize different settings for different music tracks.

Please note that the `Music_Manager` class needs an instance in the scene to operate, so make sure to add and set up the `Music_Manager` object in your Unity scene. The `PlayMusic` function will issue a warning if it cannot find an instance of `Music_Manager` in the scene.

## Music_Manager Class - Multiple Tracks Transition Feature

The `Music_Manager` class supports sequential playback of multiple music tracks with smooth volume transitions. This feature allows for a seamless audio experience by ensuring each track starts at a volume of 0 and transitions to the desired volume over a specified duration. This feature has been tested and confirmed to work as expected.

### Multiple Tracks Transition Feature

The multiple tracks transition feature is triggered when the `transitionPlay` property of the `MusicSettings` object is set to `true`. In this mode, if multiple tracks are queued to play in sequence, each track will start playing after the previous one ends, with a smooth volume transition from 0 to the desired volume as defined in `MusicSettings`.

If `transitionDuration` is set to a value greater than the duration of a track, the track will loop to allow for the full transition duration.

Here is an example of how to use the multiple tracks transition feature:

```csharp
void test3Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false; // Will not interrupt the currently playing music.
    musicSettings.transitionPlay = true; // Will use transition effects when the music starts or stops.
    musicSettings.pauseTransitions = false; // Will not use transition effects when the music is paused or resumed.
    musicSettings.transitionDuration = 3f; // The duration of the transition effect.
    musicSettings.loop = true; // The music will loop if there are no other tracks in the queue or if the transition duration is longer than the song.
    musicSettings.volume = 1; // The target volume level of the music after transition.
    musicSettings.pitch = 1; // The pitch of the music.

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings); // Will use transition effects.
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings); // Will use transition effects.
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings); // Will use transition effects.
}
```

In the example, three music tracks are loaded and queued for sequential playback, each with transition effects. This illustrates the seamless play of multiple tracks using the transition effect feature.

Please note that the `Music_Manager` class needs an instance in the scene to operate, so make sure to add and set up the `Music_Manager` object in your Unity scene. The `PlayMusic` function will issue a warning if it cannot find an instance of `Music_Manager` in the scene.

## Music_Manager Class - Single Track Transition Feature

The `Music_Manager` class supports volume transitions at the beginning and end of a single music track. This feature allows for a smooth auditory experience by ensuring each track starts from a volume of 0 and transitions to the desired volume over a specified duration at the beginning. Towards the end of the track, it transitions from the current volume back to 0. This feature has been tested and confirmed to work as expected.

### Single Track Transition Feature

The single track transition feature is activated when the `transitionPlay` property of the `MusicSettings` object is set to `true`. In this mode, the single track will play with a smooth volume transition at the start and end.

If `transitionDuration` is set to a value, the track will gradually increase its volume from 0 to the desired volume over the specified duration at the beginning, and will decrease its volume from the current volume to 0 towards the end.

Here is an example of how to use the single track transition feature:

```csharp
void test4Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false; // Will not interrupt the currently playing music.
    musicSettings.transitionPlay = true; // Will use transition effects when the music starts or stops.
    musicSettings.pauseTransitions = false; // Will not use transition effects when the music is paused or resumed.
    musicSettings.transitionDuration = 10f; // The duration of the transition effect.
    musicSettings.loop = false; // The music will not loop when it ends.
    musicSettings.volume = 1; // The target volume level of the music after transition.
    musicSettings.pitch = 1; // The pitch of the music.

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings); // Will use transition effects.
}

```

In the example, a single music track is loaded with transition effects. This illustrates the play of a single track using the transition effect feature.

Please note that the `Music_Manager` class needs an instance in the scene to operate, so make sure to add and set up the `Music_Manager` object in your Unity scene. The `PlayMusic` function will issue a warning if it cannot find an instance of `Music_Manager` in the scene.

# Music_Manager Class - Force Play Feature

The `Music_Manager` class supports a "force play" feature. When the `forcePlay` property of the `MusicSettings` object is set to `true`, the currently playing song will be immediately interrupted, and the newly requested song will start playing. The interrupted song will be placed next in the queue to resume playing after the forced song finishes. This feature has been tested and confirmed to work as expected.

## Force Play Feature

The force play feature provides a way to immediately start a song, irrespective of the current playback state. If a song is already playing when a new song is triggered with `forcePlay` set to `true`, the current song will stop, and the new song will begin. The stopped song will be placed next in the queue to be resumed once the forced song has finished.

Below is an example usage of the force play feature:

```csharp
void test5Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = true;
    musicSettings.pauseTransitions = false;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
}

void test5Update()
{
    timer += Time.deltaTime;

    if (timer > maxTime && !played)
    {
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = true; // The song will play immediately, interrupting any current song
        musicSettings2.transitionPlay = false;
        musicSettings2.transitionDuration = 20f;
        musicSettings2.loop = true;
        musicSettings2.volume = 1;
        musicSettings2.clearQueue = false;
        musicSettings2.pitch = 1;

        Music_Manager.PlayMusic("The_geese_kind_1", musicSettings2);
        played = true;
        Debug.Log("Inner Action");
    }

    // Similar operation for the second update
    if (played) timer2 += Time.deltaTime;
    if (timer2 > maxTime2 && !played2)
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = true; // The song will play immediately, interrupting any current song
        musicSettings.transitionPlay = false;
        musicSettings.transitionDuration = 2f;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
        played2 = true;
        Debug.Log("Inner Action 2");
    }
}
```

In this example, a sequence of music tracks is played using the force play feature. It demonstrates how to interrupt the current playback and start a new track immediately.

## Music_Manager Class - Force and Transition Play Feature

The `Music_Manager` class supports a "Force and Transition Play" feature. When the `forcePlay` and `transitionPlay` properties of the `MusicSettings` object are both set to `true`, the currently playing song will smoothly fade out (volume transitioning to 0) over the duration specified by `transitionDuration`. Simultaneously, the new song will start to play, transitioning its volume from 0 to the desired volume. The currently playing song will also be moved to the front of the queue. This feature has been tested and confirmed to work as expected.

### Force and Transition Play Feature

This combined feature provides a way to immediately start a song with smooth volume transitions, regardless of the current playback state. If a song is already playing when a new song is triggered with `forcePlay` and `transitionPlay` set to `true`, the current song will begin a smooth fade out, while the new song will begin with a smooth fade in.

Below is an example usage of the "Force and Transition Play" feature:

```csharp
void test6Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = false;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    Music_Manager.PlayMusic("cappy_defense_force_1", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_1", musicSettings);
}

void test6Update()
{
    timer += Time.deltaTime;

    // Below are a sequence of songs played using the "Force and Transition Play" feature
    // Each song interrupts the previous song, transitioning volume smoothly
    // Timer checks ensure that each song is allowed to play for a time before being interrupted

    // Song 1: "The_geese_kind_1"
    if (timer > maxTime && !played)
    {
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = true;
        musicSettings2.transitionPlay = true;
        musicSettings2.transitionDuration = 6f;
        musicSettings2.loop = true;
        musicSettings2.volume = 1;
        musicSettings2.clearQueue = false;
        musicSettings2.pitch = 1;

        Music_Manager.PlayMusic("The_geese_kind_1", musicSettings2);
        played = true;
        Debug.Log("Inner Action");
    }

    // Song 2: "The_geese_kind_4"
    if (played) timer2 += Time.deltaTime;
    if (timer2 > maxTime2 && !played2)
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = true;
        musicSettings.transitionPlay = true;
        musicSettings.transitionDuration = 6f;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
        played2 = true;
        Debug.Log("Inner Action 2");
    }

    // Song 3: "The_geese_kind_3"
    if (played2) timer3 += Time.deltaTime;
    if (timer3 > maxTime3 && !played3)
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = true;
        musicSettings.transitionPlay = true;
        musicSettings.transitionDuration = 6f;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);
        played3 = true;
        Debug.Log("Inner Action 3");
    }
}
    // Song 4: "cappy_defense_force_4"
    if (played3) timer4 += Time.deltaTime;
    if (timer4 > (maxTime4 * 40) && !played4)
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = true;
        musicSettings.transitionPlay = true;
        musicSettings.transitionDuration = 6f;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        Music_Manager.PlayMusic("cappy_defense_force_4", musicSettings);
        played4 = true;
        Debug.Log("Inner Action 4");
    }
```

### Conclusion

The "Force and Transition Play" feature in the `Music_Manager` class provides a powerful tool for controlling music playback in a software application. It allows for immediate switching between songs, with smooth volume transitions for a pleasant listening experience. Furthermore, it's been extensively tested and verified to work correctly in various scenarios.

## Music_Manager Class - Pause Feature

The `Music_Manager` class supports a "Pause" feature. This functionality is designed to pause the currently playing music. If the `pauseTransitions` property is set to `true` in the `MusicSettings` object, the volume of the currently playing music will smoothly transition to 0. The music will then be paused, utilizing the audio source's pause functionality. This feature has been tested and confirmed to work as expected.

### Pause Feature

The Pause feature provides a way to stop playback of the current song, with an option for a smooth volume transition to zero before the pause is initiated.

Below is an example usage of the Pause feature:

```csharp
void test2_1Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 6f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
}

void test2_1Update()
{
    timer += Time.deltaTime;
    if (timer > maxTime && !played)
    {
        // The currently playing music ("cappy_defense_force_2") will be paused,
        // with its volume transitioning to 0 over 6 seconds (as set in `transitionDuration`).
        Music_Manager.PauseMusic();
        played = true;
    }
}
```

In this test, the song "cappy_defense_force_2" is played with `pauseTransitions` set to `true` and a `transitionDuration` of 6 seconds. In the `test2_1Update` method, a timer checks for the passing of `maxTime`, and then the `PauseMusic()` method is called. This causes the music to transition its volume to 0 over the defined `transitionDuration` and then pause the playback.

## Music_Manager Class - Stop Feature

The `Music_Manager` class includes a "Stop" feature. This feature is designed to stop the currently playing music. If the `pauseTransitions` property is set to `true` in the `MusicSettings` object, the volume of the currently playing music will smoothly transition to 0. The music will then be stopped, making use of the audio source's stop function. This feature has been tested and verified to function as expected.

### Stop Feature

The Stop feature offers a method to stop the playback of the current song, providing an option for a smooth volume transition to zero before the music stops.

Here's an example of how to use the Stop feature:

```csharp
void test2_2Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
}

void test2_2Update()
{
    timer += Time.deltaTime;
    if (timer > maxTime && !played)
    {
        // The currently playing music ("cappy_defense_force_2") will be stopped,
        // with its volume transitioning to 0 over 3 seconds (as set in `transitionDuration`).
        Music_Manager.StopMusic();
        played = true;
    }
}
```

In this test, the song "cappy_defense_force_2" is played with `pauseTransitions` set to `true` and a `transitionDuration` of 3 seconds. In the `test2_2Update` method, a timer checks for the passing of `maxTime`, and then the `StopMusic()` method is called. This action causes the music to transition its volume to 0 over the defined `transitionDuration`, after which the playback is stopped.

## Music_Manager Class - Resume Feature

The `Music_Manager` class supports a "Resume" feature. This functionality allows for the continuation of the currently paused song. When the `ResumeMusic()` method is called, `audio source.play` is invoked, resuming the paused music. If the `pauseTransitions` property is set to `true` in the `MusicSettings` object, the audio will transition smoothly to the appropriate volume. This feature has been thoroughly tested and confirmed to work as intended.

### Resume Feature

The Resume feature provides a way to resume the playback of the currently paused song, with an option for a smooth volume transition from zero to the correct volume.

Here is an example of how the Resume feature can be used:

```csharp
void test2_3Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
}

void test2_3Update()
{
    timer += Time.deltaTime;
    if (timer > maxTime && !played)
    {
        Music_Manager.PauseMusic();
        played = true;
    }

    if (played) timer2 += Time.deltaTime;
    if (timer2 > maxTime2 && !played2)
    {
        // The currently paused music ("cappy_defense_force_2") will be resumed,
        // with its volume transitioning from 0 to 1 over 3 seconds (as set in `transitionDuration`).
        Music_Manager.ResumeMusic();
        played2 = true;
    }
}
```

In this test, the song "cappy_defense_force_2" is played with `pauseTransitions` set to `true` and a `transitionDuration` of 3 seconds. In the `test2_3Update` method, a timer checks for the passing of `maxTime`, and then the `PauseMusic()` method is called. Later, another timer checks for the passing of `maxTime2`, and then the `ResumeMusic()` method is invoked. This causes the music to transition its volume from 0 to the correct volume over the defined `transitionDuration` and then resume the playback.

## Music_Manager Class - Clear Queue Feature

The `Music_Manager` class provides a "Clear Queue" feature. This function enables the user to clear the queue of upcoming songs without disturbing the currently playing track. This feature is particularly useful when there is a need to change the sequence of upcoming songs on the fly. It is important to note that any looping song that may have started transitioning out before this method is called will not have its transition process halted by the `ClearQueue()` method. The feature has been tested thoroughly and confirmed to work as expected.

### Clear Queue Feature

The Clear Queue feature allows for the removal of all upcoming songs from the queue while leaving the currently playing music unaffected.

Here is an example of how the Clear Queue feature can be used:

```csharp
void test2_4Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
}

void test2_4Update()
{
    timer += Time.deltaTime;
    if (timer > (maxTime * 2) && !played)
    {
        // Clear the queue of upcoming songs.
        // The currently playing music ("cappy_defense_force_2") will continue to play undisturbed.
        Debug.Log("Clearing Queue");
        Music_Manager.ClearQueue();
        played = true;
    }
}
```

In this test, three songs are queued for playing. After `maxTime * 2` has elapsed, the `ClearQueue()` method is invoked. As a result, the two songs in the queue ("The_geese_kind_3" and "The_geese_kind_4") are removed, but the currently playing song ("cappy_defense_force_2") continues to play unaffected.

## Music_Manager Class - Clear Queue and Force Play Feature

The `Music_Manager` class includes a feature that allows the user to clear the song queue and forcefully play a new song simultaneously. The currently playing song will transition or not transition to the newly introduced song based on the transition play settings. The old song is not added to the beginning of the queue and the existing queue will be removed. The feature has been tested thoroughly and confirmed to work as expected.

### Clear Queue and Force Play Feature

This feature enables the user to clear the queue of upcoming songs and start playing a new song instantly, without disturbing the currently playing track, if the `forcePlay` parameter is set to true.

Here is an example of how to use this feature:

```csharp
void test2_5Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
}

void test2_5Update()
{
    timer += Time.deltaTime;
    if (timer > (maxTime) && !played)
    {
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = true;
        musicSettings2.transitionPlay = false;
        musicSettings2.pauseTransitions = true;
        musicSettings2.transitionDuration = 3f;
        musicSettings2.loop = true;
        musicSettings2.volume = 1;
        musicSettings2.pitch = 1;
        musicSettings2.clearQueue = true;

        // Clear the queue of upcoming songs and start playing the new song ("cappy_defense_force_1") instantly.
        Music_Manager.PlayMusic("cappy_defense_force_1", musicSettings2);
        played = true;
    }
}
```

In this test, three songs are added to the queue. After `maxTime` has passed, the `PlayMusic()` method is called with `forcePlay` set to true and `clearQueue` set to true. This triggers the currently playing song to either transition or not (based on `transitionPlay` setting) to the new song ("cappy_defense_force_1"), and clears the queue of any upcoming songs ("The_geese_kind_3" and "The_geese_kind_4"). The old song ("cappy_defense_force_2") is not added back to the queue.

## Music_Manager Class - Clear Queue without Force Play Feature

The `Music_Manager` class features a Clear Queue option that when combined with a `forcePlay` setting of `false`, still allows for the removal of all upcoming songs from the queue while issuing a warning log to clarify the action taken. The functionality has been tested thoroughly and confirmed to work as expected.

### Clear Queue without Force Play Feature

This feature enables the user to clear the queue of upcoming songs and initiate the playing of a new song without forcing a transition from the current song if the `forcePlay` parameter is set to `false`. In such cases, a warning log is issued to explain the result to the user.

Here's how you can use this feature:

```csharp
void test2_6Start()
{
    MusicSettings musicSettings = new MusicSettings();
    musicSettings.forcePlay = false;
    musicSettings.transitionPlay = false;
    musicSettings.pauseTransitions = true;
    musicSettings.transitionDuration = 3f;
    musicSettings.loop = true;
    musicSettings.volume = 1;
    musicSettings.pitch = 1;

    Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);
    Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
}

void test2_6Update()
{
    timer += Time.deltaTime;
    if (timer > (maxTime) && !played)
    {
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = false;
        musicSettings2.transitionPlay = false;
        musicSettings2.pauseTransitions = true;
        musicSettings2.transitionDuration = 3f;
        musicSettings2.loop = true;
        musicSettings2.volume = 1;
        musicSettings2.pitch = 1;
        musicSettings2.clearQueue = true;

        // Clear the queue of upcoming songs and start playing the new song ("cappy_defense_force_1") without forcing the transition.
        Music_Manager.PlayMusic("cappy_defense_force_1", musicSettings2);
        played = true;
    }
}
```

In this test, three songs are added to the queue. After `maxTime` has passed, the `PlayMusic()` method is called with `forcePlay` set to `false` and `clearQueue` set to `true`. The queue of upcoming songs
