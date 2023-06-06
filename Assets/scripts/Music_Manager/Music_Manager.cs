using System.Collections.Generic;
using UnityEngine;
using System;

/* 
    Music transition system, when a force play transition is played, 
    the current song transitions to lower volume. The next music clip
    is then played at 0 volume and then transitions to full volume.

    When the current music request is close to finishing and it has a property from 
    the MusicSettings struct called transitionPlay set to true, when that song approaches
    the end of its duration, that song will transition to volume 0 nad the next song in the
    queue will play at volume 0 and then transition to full volume if that next song has 
    transitionPlay set to true.
*/

public class Music_Manager : MonoBehaviour
{
    private static Music_Manager instance;
    private AudioSource audioSource;
    private Queue<MusicRequest> musicQueue = new Queue<MusicRequest>();
    private int previousSamplePosition = 0;
    MusicRequest lastFramesCurrentMusic;

    //TODO: Make private when finished debugging
    bool transitioning = false;

    bool halted = false;

    bool overFlowTransitioning = false;

    bool isFocused = true;

    MusicRequest currentMusic;

    MusicRequest? transitionSongBuffer;
    MusicRequest? initialTransitionSongBuffer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public static void PlayMusic(string musicName, MusicSettings musicSettings)
    {
        // If there is bugs with transitions, its likely due to this line LOOK HERE TAG
        instance.halted = false;
        if (instance == null)
        {
            Debug.LogWarning("Music_Manager instance not found in the scene.");
            return;
        }

        AudioClip musicClip = Resources.Load<AudioClip>("music/" + musicName);
        MusicRequest newMusic = new(musicClip, musicSettings);

        if (instance.audioSource.isPlaying)
        {
            if (musicSettings.clearQueue)
            {
                if (!musicSettings.forcePlay) Debug.LogWarning("When clearQueue is true in settings object, the music will always be force played");
                instance.immediateChangeMusic(newMusic);
                ClearQueue();

            }
            else if (musicSettings.forcePlay)
                instance.immediateChangeMusic(newMusic);
            else
                instance.musicQueue.Enqueue(newMusic);
        }
        else
        {
            instance.PlayMusicRequest(newMusic);
            instance.currentMusic = newMusic;
        }
    }

    void immediateChangeMusic(MusicRequest newMusic)
    {
        MusicSettings musicSettings = newMusic.musicSettings;
        if (musicSettings.transitionPlay)
            instance.forceTransitionPlay(newMusic);
        else
            instance.forcePlay(newMusic);
    }
    void forcePlay(MusicRequest newMusic)
    {
        /* #if UNITY_EDITOR
                    EditorApplication.isPaused = true;
        #endif */
        if (!instance.transitioning)
            EnqueueAtBeginning(instance.currentMusic);
        instance.currentMusic = newMusic;
        instance.audioSource.clip = newMusic.musicClip;
        instance.audioSource.Play();
    }
    void forceTransitionPlay(MusicRequest newMusic)
    {

        MusicSettings musicSettings = newMusic.musicSettings;
        if (!instance.transitioning)
        {
            if (!object.ReferenceEquals(instance.transitionSongBuffer, null))
            {
                EnqueueAtBeginning((MusicRequest)instance.transitionSongBuffer);
                instance.transitionSongBuffer = null;
            }
            EnqueueAtBeginning(instance.currentMusic);
            instance.initialTransitionSongBuffer = newMusic;
        }
        else
        {
            if (!object.ReferenceEquals(instance.initialTransitionSongBuffer, null))
            {

                EnqueueAtBeginning((MusicRequest)instance.initialTransitionSongBuffer);
                instance.initialTransitionSongBuffer = null;
            }
            if (!object.ReferenceEquals(instance.transitionSongBuffer, null))
            {
                EnqueueAtBeginning((MusicRequest)instance.transitionSongBuffer);
            }
            instance.transitionSongBuffer = newMusic;
        }
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.TransitionVolume(1, 0, musicSettings.transitionDuration, () =>
        {
            instance.currentMusic = newMusic;
            instance.PlayMusicRequest(newMusic);
        }));
    }
    public static void ResumeMusic()
    {
        if (instance == null) return;
        instance.audioSource.Play();
        instance.halted = false;
        if (instance.currentMusic.musicSettings.pauseTransitions)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration));
        }
    }
    public static void PauseMusic()
    {
        if (instance == null) return;
        instance.halted = true;
        if (instance.currentMusic.musicSettings.pauseTransitions)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration, () =>
            {
                instance.audioSource.Pause();
            }));
        }
        else
            instance.audioSource.Pause();
    }

    public static void StopMusic()
    {
        if (instance == null) return;

        instance.halted = true;

        if (instance.currentMusic.musicSettings.pauseTransitions)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration, () =>
            {
                instance.audioSource.Stop();
            }));
        }
        else
            instance.audioSource.Stop();

    }

    static void EnqueueAtBeginning(MusicRequest value)
    {
        // Create a temporary queue and add the new value
        Queue<MusicRequest> tempQueue = new Queue<MusicRequest>();
        tempQueue.Enqueue(value);

        // Add all items from the original queue to the temporary queue
        while (instance.musicQueue.Count > 0)
            tempQueue.Enqueue(instance.musicQueue.Dequeue());

        // Replace the original queue with the temporary queue
        instance.musicQueue = tempQueue;
    }


    public static void ClearQueue()
    {
        if (instance == null) return;
        instance.musicQueue.Clear();
    }

    private void PlayMusicRequest(MusicRequest request)
    {
        audioSource.clip = request.musicClip;
        audioSource.loop = request.musicSettings.loop;
        audioSource.volume = request.musicSettings.volume;
        audioSource.pitch = request.musicSettings.pitch;

        // If there is bugs with transitions, its likely due to this line LOOK HERE TAG
        instance.halted = false;

        if (request.musicSettings.transitionPlay)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            {
                instance.StopAllCoroutines();
                StartCoroutine(TransitionVolume(0, 1, request.musicSettings.transitionDuration));
            }
        }
        else
        {
            audioSource.Play();
        }
    }

    private IEnumerator<WaitForSeconds> waitAFrame(Action done = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            elapsedTime += 1;
            yield return null;
        }

        done?.Invoke();
    }

    private IEnumerator<WaitForSeconds> TransitionVolume(float startVolume, float targetVolume, float transitionTime, Action done = null)
    {
        if (!instance.audioSource.clip)
            yield return null;

        // TODO: Check if transitionTime is greater than the time remaining in the current music clip.
        if (instance.audioSource.clip.length - instance.audioSource.time < transitionTime && !instance.transitioning)
        {
            instance.overFlowTransitioning = true;
            /* transitionTime = instance.audioSource.clip.length - instance.audioSource.time; */
        }
        else
        {
            instance.overFlowTransitioning = false;
        }

        float calculatedStartVolume = startVolume;
        if (instance.transitioning) calculatedStartVolume = audioSource.volume;

        instance.transitioning = true;

        audioSource.volume = calculatedStartVolume;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(calculatedStartVolume, targetVolume, elapsedTime / transitionTime);
            yield return null; // Wait for the next frame to continue the loop.
        }

        audioSource.volume = targetVolume; // Ensure the volume is exactly the target volume at the end of the transition.

        instance.transitioning = false;
        done?.Invoke();
    }

    /* 
        Determines weather or not the current music clip should 
        and is ready to transition to volume 0. 
    */
    private bool IsTransitionReady()
    {
        if (!instance.audioSource.clip) return false;
        bool isTransitionPlay = instance.currentMusic.musicSettings.transitionPlay;
        bool isDurationReady = instance.audioSource.clip.length - instance.audioSource.time < instance.currentMusic.musicSettings.transitionDuration;
        bool isLoopConditionMet = !instance.currentMusic.musicSettings.loop || (instance.currentMusic.musicSettings.loop && instance.musicQueue.Count > 0);

        return isTransitionPlay && isDurationReady && isLoopConditionMet && !instance.overFlowTransitioning && !instance.transitioning;
    }

    private bool IsMusicReady()
    {
        bool hasMusicStopped = !audioSource.isPlaying && instance.isFocused && musicQueue.Count > 0;

        //True when music has looped and there is other music in the queue
        bool hasMusicLooped = audioSource.isPlaying && audioSource.timeSamples < previousSamplePosition && musicQueue.Count > 0 && (instance.currentMusic.ToString() == instance.lastFramesCurrentMusic.ToString());


        bool transitionReady = !instance.transitioning && !instance.halted;

        return (hasMusicStopped || hasMusicLooped || instance.overFlowTransitioning) && transitionReady;
    }

    private void Update()
    {
        CheckMusicState();
    }

    public void CheckMusicState()
    {
        if (IsTransitionReady())
        {
            /* 
                Theory for bug. 
                Sometimes the transitionDuration is greater than the time remaining in the current music clip. 
                This makes it so that the instance transition property stays true during the short window where 
                the loop detection is true. This makes it so that the next song is not played when the 
                transition is finished. 
            */
            instance.StopAllCoroutines();
            StartCoroutine(TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration));
        }

        if (IsMusicReady())
        {
            instance.overFlowTransitioning = false;
            /* if (!object.ReferenceEquals(instance.transitionSongBuffer, null))
            {
                EnqueueAtBeginning((MusicRequest)instance.transitionSongBuffer);
                instance.transitionSongBuffer = null;
            } */
            if (musicQueue.Count > 0)
            {
                MusicRequest nextRequest = musicQueue.Dequeue();
                PlayMusicRequest(nextRequest);
                instance.currentMusic = nextRequest;
            }

        }

        previousSamplePosition = audioSource.timeSamples;
        lastFramesCurrentMusic = instance.currentMusic;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        instance.isFocused = hasFocus;
    }
}

[System.Serializable]
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

    public override string ToString()
    {
        return "MusicSettings: " + forcePlay + " " + transitionPlay + " " + transitionDuration + " " + loop + " " + volume + " " + pitch;
    }
}

struct MusicRequest
{
    public AudioClip musicClip;
    public MusicSettings musicSettings;

    public MusicRequest(AudioClip clip, MusicSettings settings)
    {
        musicClip = clip;
        musicSettings = settings;
    }

    public override string ToString()
    {
        return "MusicRequest: " + this.musicClip + " " + this.musicSettings;
    }
}