using System.Collections.Generic;
using UnityEngine;

/* 
    Music transition system, when a force play transition is played, 
    the current song transitions to lower volume. The next music clip
    is then played at 0 volume and then transitions to full volume.

    When the current music request is close to finishing and it has a property from 
    the MusicSettings struct called transitionPlay set to true, when that song approaches
    the end of its duration, that song will transition to volume 0 nad the next song in the
    queue will play at volume 0 and then transition to full volume if that next song has 
    transitionPlay set.
*/

public class Music_Manager : MonoBehaviour
{
    private static Music_Manager instance;
    private AudioSource audioSource;
    private Queue<MusicRequest> musicQueue = new Queue<MusicRequest>();
    private int previousSamplePosition = 0;

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

        if (instance.audioSource.isPlaying)
        {
            if (musicSettings.forcePlay)
            {
                if (musicSettings.transitionPlay)
                {
                    // Dose not work.
                    EnqueueAtBeginning(new MusicRequest(musicClip, musicSettings));
                    instance.musicQueue.Enqueue(new MusicRequest(musicClip, musicSettings));
                    instance.StartCoroutine(instance.TransitionPlay(musicSettings.transitionDuration));
                }
                else
                {
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
        audioSource.clip = request.musicClip;
        audioSource.loop = request.musicSettings.loop;
        audioSource.volume = request.musicSettings.volume;
        audioSource.pitch = request.musicSettings.pitch;

        if (request.musicSettings.transitionPlay && audioSource.isPlaying)
        {
            StartCoroutine(TransitionPlay(request.musicSettings.transitionDuration));
        }
        else
        {
            audioSource.Play();
        }
    }

    private IEnumerator<WaitForSeconds> TransitionPlay(float transitionDuration)
    {
        float t = 0;

        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, audioSource.volume, t / transitionDuration);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        audioSource.volume = 1;
        audioSource.Play();
    }

    private void Update()
    {
        manageMusicQueue();
    }

    void manageMusicQueue(){
        /* determines weather or not to play the next music in the queue */
        if((audioSource.isPlaying && audioSource.timeSamples < previousSamplePosition && musicQueue.Count > 0) || (!audioSource.isPlaying && musicQueue.Count > 0)){
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