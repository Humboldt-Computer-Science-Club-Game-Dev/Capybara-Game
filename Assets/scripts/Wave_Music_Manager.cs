using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Music_Manager : MonoBehaviour
{
    public MusicActionSet[] musicActionSets;
    int currentWave = -1;

    void Start()
    {
        Event_System.onWaveEnds += onWaveEnds;
        Event_System.onSequenceEnds += onSequenceEnds;
    }

    void onWaveEnds(int waveIndex)
    {
        currentWave = waveIndex;
        for (int i = 0; i < musicActionSets.Length; i++)
        {
            MusicActionSet currentSet = musicActionSets[i];
            if (currentSet.playOnWaveIndex != waveIndex || currentSet.playOnSequenceEnd) continue;
            MusicAction[] actions = currentSet.actions;
            for (int j = 0; j < actions.Length; j++)
            {
                MusicAction currentAction = actions[j];
                switch (currentAction.actionType)
                {
                    case MusicActionType.play:
                        Music_Manager.PlayMusic(currentAction.musicName, currentAction.musicSettings);
                        break;
                    case MusicActionType.stop:
                        Music_Manager.StopMusic();
                        break;
                    case MusicActionType.pause:
                        Music_Manager.PauseMusic();
                        break;
                    case MusicActionType.resume:
                        Music_Manager.ResumeMusic();
                        break;
                    case MusicActionType.clear:
                        Music_Manager.ClearQueue();
                        break;
                }
            }
        }
    }

    void onSequenceEnds()
    {
        Debug.Log("Sequence ends");
        for (int i = 0; i < musicActionSets.Length; i++)
        {
            MusicActionSet currentSet = musicActionSets[i];
            if (currentWave != currentSet.playOnWaveIndex || !currentSet.playOnSequenceEnd) continue;
            MusicAction[] actions = currentSet.actions;
            for (int j = 0; j < actions.Length; j++)
            {
                MusicAction currentAction = actions[j];
                switch (currentAction.actionType)
                {
                    case MusicActionType.play:
                        Music_Manager.PlayMusic(currentAction.musicName, currentAction.musicSettings);
                        break;
                    case MusicActionType.stop:
                        Music_Manager.StopMusic();
                        break;
                    case MusicActionType.pause:
                        Music_Manager.PauseMusic();
                        break;
                    case MusicActionType.resume:
                        Music_Manager.ResumeMusic();
                        break;
                    case MusicActionType.clear:
                        Music_Manager.ClearQueue();
                        break;
                }
            }
        }
    }

    [System.Serializable]
    public struct MusicAction
    {
        public MusicActionType actionType;
        public string musicName;
        public MusicSettings musicSettings;
    }

    [System.Serializable]
    public struct MusicActionSet
    {
        public MusicAction[] actions;
        public int playOnWaveIndex;
        public bool playOnSequenceEnd;
    }

    public enum MusicActionType
    {
        play,
        stop,
        pause,
        resume,
        clear
    }

}
