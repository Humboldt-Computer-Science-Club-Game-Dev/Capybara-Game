using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_System : MonoBehaviour
{
    private static Dialog_System instance;
    public Sequence[] Sequences;
    public Wave_System wave_System;
    List<Sequence> currentSequences = new List<Sequence>();

    public OnSequencesEnd onSequencesEnd;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        if (wave_System == null)
        {
            GameObject wave_System_GameObject = GameObject.Find("wave_system");
            if (wave_System_GameObject != null)
                wave_System = wave_System_GameObject.GetComponent<Wave_System>();
        }

        dialogEvent(PlayOnOptions.Start);
    }


    void Update()
    {

    }

    public static void dialogEvent(PlayOnOptions playOnOption)
    {
        if (playOnOption == PlayOnOptions.Start)
        {
            instance.currentSequences.Clear();
            instance.sequencesEnded();
        }
        else if (playOnOption == PlayOnOptions.EndOfAllWaves)
        {

        }
        else if (playOnOption == PlayOnOptions.WaveNumber)
            Debug.LogError("Canot pass in type PlayOnOptions whos value is WaveNumber. Please pass in an int instead to the dialogEvent function if you want Dialog_System to play a dialog sequence based on the wave number.");

    }

    public static void dialogEvent(int waveNumber)
    {

    }

    void sequencesEnded()
    {
        instance.wave_System.spawnNextWave();
    }

    public static void setCustomPostSequenceAction(System.Action action)
    {

    }
}

public enum EnvironmentAction
{
    newBackground,
    keepSame,
    makeNothing
}

[System.Serializable]
public struct Environment
{
    public EnvironmentAction environmentAction;

    public Sprite newBackground;

}

public enum CharacterDialogPosition
{
    left,
    right
}

[System.Serializable]
public struct Dialog
{
    public string name;
    public string text;
    public AudioClip voice;
    public CharacterDialogPosition position;
    public Sprite character;
}

public enum MusicActionAction
{
    noChange,
    runAction
}

public enum MusicRunAction
{
    play,
    stop,
    pause,
    resume,
    clear
}



[System.Serializable]
public struct Music_Action
{
    public MusicActionAction musicActionAction;
    public MusicRunAction musicRunAction;
    public string musicName;
    public MusicSettings musicSettings;
}

[System.Serializable]
public struct Entire
{
    public Environment environment;
    public Dialog dialog;
    public Music_Action music_Action;
}


public enum PlayOnOptions
{
    Start,
    EndOfAllWaves,
    WaveNumber,
}

[System.Serializable]
public struct PlayOn
{
    public PlayOnOptions playOnOption;
    public int waveNumber;
}

[System.Serializable]
public struct Sequence
{
    public Entire[] entries;
    public PlayOn playOn;
}

public enum OnSequencesEnd
{
    EndLevel,
    NextWave,
    Custom
}