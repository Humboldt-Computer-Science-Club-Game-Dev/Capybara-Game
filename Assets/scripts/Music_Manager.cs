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
    public bool transitioning = false;

    public bool halted = false;

    bool overFlowTransitioning = false;

    bool isFocused = true;

    MusicRequest currentMusic;

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
                instance.ClearQueue();
                immediateChangeMusic(newMusic);
            }
            else if (musicSettings.forcePlay)
                immediateChangeMusic(newMusic);
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
        MusicSettings = newMusic.musicSettings;
        if (musicSettings.transitionPlay)
            instance.forceTransitionPlay(newMusic);
        else
            instance.forcePlay(newMusic);
    }
    void forcePlay(MusicRequest newMusic)
    {
        Debug.Log("Force Playing");
        /* #if UNITY_EDITOR
                    EditorApplication.isPaused = true;
        #endif */
        EnqueueAtBeginning(instance.currentMusic);
        instance.currentMusic = newMusic;
        instance.audioSource.clip = newMusic.musicClip;
        instance.audioSource.Play();
    }
    void forceTransitionPlay(MusicRequest newMusic)
    {
        MusicSettings musicSettings = newMusic.musicSettings;
        EnqueueAtBeginning(instance.currentMusic);
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
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration));

    }
    public static void PauseMusic()
    {
        if (instance == null) return;
        instance.halted = true;
        if (instance.currentMusic.musicSettings.pauseTransitions)
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration, () =>
            {
                instance.audioSource.Pause();
            }));
        else
            instance.audioSource.Pause();
    }

    public static void StopMusic()
    {
        if (instance == null) return;

        instance.halted = true;

        if (instance.currentMusic.musicSettings.pauseTransitions)
            instance.StartCoroutine(instance.TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration, () =>
            {
                instance.audioSource.Stop();
            }));
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

        if (request.musicSettings.transitionPlay)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            StartCoroutine(TransitionVolume(0, 1, request.musicSettings.transitionDuration));
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
        if (instance.transitioning) yield return null;
        instance.transitioning = true;

        // TODO: Check if transitionTime is greater than the time remaining in the current music clip.
        if (instance.audioSource.clip.length - instance.audioSource.time < transitionTime)
        {
            instance.overFlowTransitioning = true;
            /* transitionTime = instance.audioSource.clip.length - instance.audioSource.time; */
        }

        audioSource.volume = startVolume;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / transitionTime);
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
        bool isTransitionPlay = instance.currentMusic.musicSettings.transitionPlay;
        bool isDurationReady = instance.audioSource.clip.length - instance.audioSource.time < currentMusic.musicSettings.transitionDuration;
        bool isLoopConditionMet = !instance.currentMusic.musicSettings.loop || (instance.currentMusic.musicSettings.loop && instance.musicQueue.Count > 0);


        /* 
            Potential bug. It should not be necessary that a check for !instance.overFlowTransitioning is made. It should
            be discoverd why its necessary to check if the current music clip has overflown its transition time because
            !instance.transitioning should be enough to check if the current music clip is still transitioning.

            Possible reason. instance.transitioning gets set to false when the current music clip is done transitioning. but one of these
            other conditions is still true. This makes it so the transition starts again when the current music clip is done transitioning.
            !instance.overFlowTransitioning is set to true when the transition starts and is set to false when the transition is done. This makes
            explains why it is needed to make this work. To fix this, I should analyze my code and see if can find out why the other conditions are still
            true when the current music clip is done transitioning. In fact, isDurationReady may be what is causing the problem. It may be that the
            current music clip is done transitioning but isDurationReady is still true. This makes it so that the transition starts again when the current
            music clip is done transitioning. Because of this, the best solution will likely be to rename overFlowTransitioning to refelect the other
            job it dose.
        */
        return isTransitionPlay && isDurationReady && isLoopConditionMet && !instance.overFlowTransitioning && !instance.transitioning;
    }

    private bool IsMusicReady()
    {
        bool hasMusicStopped = !audioSource.isPlaying && instance.isFocused && musicQueue.Count > 0;

        //True when music has looped and there is other music in the queue
        bool hasMusicLooped = audioSource.isPlaying && audioSource.timeSamples < previousSamplePosition && musicQueue.Count > 0 && (instance.currentMusic.ToString() == instance.lastFramesCurrentMusic.ToString());

        bool transitionReady = !instance.transitioning && !instance.halted;

        if ((hasMusicStopped || hasMusicLooped || instance.overFlowTransitioning) && transitionReady)
        {
            Debug.Log("hasMusicStopped: " + hasMusicStopped + " hasMusicLooped: " + hasMusicLooped + " instance.overFlowTransitioning: " + instance.overFlowTransitioning + " transitionReady: " + transitionReady);
        }
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
            StartCoroutine(TransitionVolume(1, 0, instance.currentMusic.musicSettings.transitionDuration));
        }

        if (IsMusicReady())
        {
            Debug.Log("Playing next");
            instance.overFlowTransitioning = false;
            MusicRequest nextRequest = musicQueue.Dequeue();
            PlayMusicRequest(nextRequest);
            instance.currentMusic = nextRequest;
        }

        previousSamplePosition = audioSource.timeSamples;
        lastFramesCurrentMusic = instance.currentMusic;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        instance.isFocused = hasFocus;
    }
}

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