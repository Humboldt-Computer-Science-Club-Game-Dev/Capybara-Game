using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    private static Music_Manager instance;
    private AudioSource audioSource;
    private Queue<MusicRequest> musicQueue = new Queue<MusicRequest>();

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

        AudioClip musicClip = Resources.Load<AudioClip>("Music/" + musicName);

        if (instance.audioSource.isPlaying)
        {
            if (musicSettings.forcePlay)
            {
                if (musicSettings.transitionPlay)
                {
                    instance.musicQueue.Enqueue(new MusicRequest(musicClip, musicSettings));
                    StartCoroutine(instance.TransitionPlay(musicSettings.transitionDuration));
                }
                else
                {
                    instance.musicQueue.Enqueue(new MusicRequest(instance.audioSource.clip, musicSettings));
                    instance.audioSource.clip = musicClip;
                    instance.audioSource.Play();
                }
            }
            else
            {
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
        audioSource.clip = request.MusicClip;
        audioSource.loop = request.MusicSettings.loop;
        audioSource.volume = request.MusicSettings.volume;
        audioSource.pitch = request.MusicSettings.pitch;

        if (request.MusicSettings.transitionPlay && audioSource.isPlaying)
        {
            StartCoroutine(TransitionPlay(request.MusicSettings.transitionDuration));
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
        if (audioSource.isPlaying || musicQueue.Count == 0) return;

        MusicRequest nextRequest = musicQueue.Dequeue();
        PlayMusicRequest(nextRequest);
    }

    private struct MusicRequest
    {
        public AudioClip musicClip;
        public MusicSettings musicSettings;

        public MusicRequest(AudioClip clip, MusicSettings settings)
        {
            musicClip = clip;
            musicSettings = settings;
        }
    }
}

public struct MusicSettings
{
    public bool forcePlay;
    public bool transitionPlay;
    public float transitionDuration;
    public bool shouldWaitUntilFinished;
}