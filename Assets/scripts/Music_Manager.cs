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
    bool transitioning = false;

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
        MusicRequest newMusic = new (musicClip, musicSettings);

        if (instance.audioSource.isPlaying)
        {
            if (musicSettings.forcePlay)
            {
                if (musicSettings.transitionPlay)
                {
                    // Dose not work.
                    EnqueueAtBeginning(instance.currentMusic);
                    //Should wait for transition to finish before running next line of code.
                    Debug.Log("Start of transition");
                    instance.StartCoroutine(instance.TransitionVolume(1, 0, musicSettings.transitionDuration, () => {
                        Debug.Log("Callback");
                        instance.currentMusic = newMusic;
                        instance.PlayMusicRequest(newMusic);
                    }));
                    
                    
                    
                }
                else
                {
                    instance.currentMusic = newMusic;
                    instance.audioSource.clip = musicClip;
                    instance.audioSource.Play();
                }
            }
            else
            {
                Debug.Log("Music_Manager: " + musicName + " added to queue.");
                instance.musicQueue.Enqueue(new MusicRequest(musicClip, musicSettings));
            }
        }
        else
        {
            instance.PlayMusicRequest(new MusicRequest(musicClip, musicSettings));
        }
    }

    public static void PauseMusic()
    {
        if (instance == null)
        {
            Debug.LogWarning("Music_Manager instance not found in the scene.");
            return;
        }

        instance.audioSource.Pause();
    }

    public static void StopMusic()
    {
        if (instance == null)
        {
            Debug.LogWarning("Music_Manager instance not found in the scene.");
            return;
        }

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
        if (instance == null)
        {
            Debug.LogWarning("Music_Manager instance not found in the scene.");
            return;
        }

        instance.musicQueue.Clear();
    }

    private void PlayMusicRequest(MusicRequest request)
    {
        Debug.Log("Playing Music Request");
        audioSource.clip = request.musicClip;
        audioSource.loop = request.musicSettings.loop;
        audioSource.volume = request.musicSettings.volume;
        audioSource.pitch = request.musicSettings.pitch;

        if (request.musicSettings.transitionPlay)
        {
            if(!audioSource.isPlaying) audioSource.Play();
            StartCoroutine(TransitionVolume(0, 1, request.musicSettings.transitionDuration));
        }
        else
        {
            audioSource.Play();
        }
    }

    private IEnumerator<WaitForSeconds> TransitionVolume(float startVolume, float targetVolume, float transitionTime, Action done = null)
    {
        if(instance.transitioning) yield return null;
        instance.transitioning = true;

        // TODO: Check if transitionTime is greater than the time remaining in the current music clip.
        audioSource.volume = startVolume;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / transitionTime);
            yield return null; // Wait for the next frame to continue the loop.
        }

        Debug.Log("End of transition");

        audioSource.volume = targetVolume; // Ensure the volume is exactly the target volume at the end of the transition.

        instance.transitioning = false;
        done?.Invoke();
    }

    private void Update()
    {
        manageMusicQueue();
    }

    void manageMusicQueue(){
        /* determines weather or not to play the next music in the queue */
        //TODO: Make this if statement, barible
        if(((audioSource.isPlaying && audioSource.timeSamples < previousSamplePosition && musicQueue.Count > 0) || (!audioSource.isPlaying && musicQueue.Count > 0)) && !instance.transitioning){
            Debug.Log("Music_Manager: playing next music in queue.");
            MusicRequest nextRequest = musicQueue.Dequeue();
            PlayMusicRequest(nextRequest);
        }
        previousSamplePosition = audioSource.timeSamples;
    }

    
}

public struct MusicSettings
{
    public bool forcePlay;
    public bool transitionPlay;
    public float transitionDuration;
    public bool loop;
    public float volume;
    public float pitch;
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
}