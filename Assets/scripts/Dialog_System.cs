using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog_System : MonoBehaviour
{
    public Sequence[] Sequences;

    List<Sequence> currentSequences = new List<Sequence>();

    public OnSequencesEnd onSequencesEnd;

    Wave_System wave_System;

    ObjectsAndComponents objectsAndComponents;
    int sequenceIndex;
    int entireIndex;
    System.Action customPostSequenceAction;

    AudioSource audioSource;

    Camera mainCamera;

    bool started = false;

    bool decommissionMe = false;

    void Awake()
    {
       
    }

    void Start()
    {
        if (started) return;
        getComponents();
        objectsAndComponents = new ObjectsAndComponents(this.gameObject);
        objectsAndComponents.hide();

        Event_System.onWaveEnds += waveEnded;
        Event_System.onLastWaveFinished += lastWaveFinished;

        dialogEvent(PlayOnOptions.Start);
        started = true;
    }

    void Update()
    {

    }

    void getComponents()
    {
        if (wave_System == null)
        {
            GameObject wave_System_GameObject = GameObject.Find("wave_system");
            if (wave_System_GameObject != null)
                wave_System = wave_System_GameObject.GetComponent<Wave_System>();
        }

        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    public void dialogEvent(PlayOnOptions playOnOption)
    {
        currentSequences.Clear();
        if (playOnOption == PlayOnOptions.Start || playOnOption == PlayOnOptions.EndOfAllWaves)
        {
            foreach (Sequence sequence in Sequences)
            {
                if (sequence.playOn.playOnOption == playOnOption)
                    currentSequences.Add(sequence);
            }
            if (currentSequences.Count > 0) startSequences();
            else sequencesEnded();
        }
        else if (playOnOption == PlayOnOptions.WaveNumber)
        {
            Debug.LogError("Can not pass in type PlayOnOptions whose value is PlayOnOptions.WaveNumber. Please pass in an int instead to the dialogEvent function if you want Dialog_System to play a dialog sequence based on the wave number.");
            return;
        }
    }

    void lastWaveFinished(){
        if(decommissionMe) return;
        decommissionMe = true;
        onSequencesEnd = OnSequencesEnd.EndLevel;
        Debug.Log("lastWaveFinished. The very last stuff is getting ran");
        dialogEvent(PlayOnOptions.EndOfAllWaves);
    }

    void waveEnded(int endedWaveIndex)
    {
        if (!started) Start();

        // An index of -1 means the wave manager has started. This would conflict with how the dialog system starts if we ran this index.
        if (endedWaveIndex == -1) return;

        dialogEvent(endedWaveIndex + 1);
    }
    public void dialogEvent(int waveNumberIndex)
    {
        currentSequences.Clear();
        foreach (Sequence sequence in Sequences)
        {
            if (sequence.playOn.playOnOption == PlayOnOptions.WaveNumber && sequence.playOn.waveNumberIndex == waveNumberIndex)
                currentSequences.Add(sequence);
        }
        if (currentSequences.Count > 0) startSequences();
        else sequencesEnded();
    }

    void startSequences()
    {
        sequenceIndex = 0;
        entireIndex = 0;

        runCurrentEntire();

        objectsAndComponents.show();
    }

    void runCurrentEntire()
    {
        displayCurrentEntire();
        playCurrentEntireVoice();
        runMusicAction();
    }

    void runMusicAction()
    {
        Entire currentEntire = currentSequences[sequenceIndex].entries[entireIndex];
        if (currentEntire.music_Action.musicActionAction == MusicActionAction.noChange)
            return;

        if (currentEntire.music_Action.musicRunAction == MusicRunAction.play)
            Music_Manager.PlayMusic(currentEntire.music_Action.musicName, currentEntire.music_Action.musicSettings);
        else if (currentEntire.music_Action.musicRunAction == MusicRunAction.stop)
            Music_Manager.StopMusic();
        else if (currentEntire.music_Action.musicRunAction == MusicRunAction.pause)
            Music_Manager.PauseMusic();
        else if (currentEntire.music_Action.musicRunAction == MusicRunAction.resume)
            Music_Manager.ResumeMusic();
        else if (currentEntire.music_Action.musicRunAction == MusicRunAction.clear)
            Music_Manager.ClearQueue();

    }

    void displayCurrentEntire()
    {
        Entire currentEntire = currentSequences[sequenceIndex].entries[entireIndex];

        if (currentEntire.environment.environmentAction == EnvironmentAction.newBackground)
            objectsAndComponents.backgroundImage.sprite = currentEntire.environment.newBackground;
        displaySubject();
        objectsAndComponents.subjectName.text = currentEntire.dialog.name;
        objectsAndComponents.dialog.text = currentEntire.dialog.text;
    }

    void displaySubject()
    {
        //TODO: Clean this function up
        Entire currentEntire = currentSequences[sequenceIndex].entries[entireIndex];
        CharacterDialogPosition position = currentEntire.dialog.position;
        Sprite subject = currentEntire.dialog.character;
        if(!subject) {
            objectsAndComponents.subjectImage.sprite = null;
            objectsAndComponents.subjectImage.enabled = false;
            return;
        }

        Image spriteRenderer = objectsAndComponents.subjectImage;
        RectTransform imageRectTransform = spriteRenderer.rectTransform;
        float scalePercentage = 0.9f;
        objectsAndComponents.subjectImage.enabled = true;
        objectsAndComponents.subjectImage.sprite = subject;


        float screenHeightInWorldUnits = mainCamera.orthographicSize * 2;
        float screenWidthInWorldUnits = screenHeightInWorldUnits * mainCamera.aspect;

        float spriteAspect = subject.bounds.size.x / subject.bounds.size.y;


        float aspectRatio = subject.rect.width / subject.rect.height;

        float potentialNewUnadjustedHight = (Screen.width * scalePercentage);

        float potentialNewHight = (potentialNewUnadjustedHight / aspectRatio) / transform.localScale.x;

        if (subject.rect.height > Screen.height || potentialNewHight > subject.rect.width)
        {
            // Set height to 80% of screen height, adjust width based on aspect ratio.
            float newHeight = Screen.height * scalePercentage;
            imageRectTransform.sizeDelta = new Vector2((newHeight * aspectRatio) / transform.localScale.x, (newHeight) / transform.localScale.x);
        }
        else
        {
            // Set width to 80% of screen width, adjust height based on aspect ratio.
            float newWidth = Screen.width * scalePercentage;
            imageRectTransform.sizeDelta = new Vector2((newWidth) / transform.localScale.x, (newWidth / aspectRatio) / transform.localScale.x);
        }


        if (currentEntire.dialog.position == CharacterDialogPosition.left)
        {
            imageRectTransform.eulerAngles = new Vector3(0, 0, 0);

            imageRectTransform.anchorMin = new Vector2(0, 0);
            imageRectTransform.anchorMax = new Vector2(0, 0);
            imageRectTransform.pivot = new Vector2(0, 0);
            imageRectTransform.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            imageRectTransform.eulerAngles = new Vector3(0, 180, 0);

            // Move to bottom right of screen
            imageRectTransform.anchorMin = new Vector2(1, 0);
            imageRectTransform.anchorMax = new Vector2(1, 0);
            imageRectTransform.pivot = new Vector2(0, 0);
            imageRectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    void playCurrentEntireVoice()
    {
        Entire currentEntire = currentSequences[sequenceIndex].entries[entireIndex];
        audioSource.clip = currentEntire.dialog.voice;
        audioSource.Play();
    }

    public void nextEntire()
    {
        entireIndex++;
        if (entireIndex >= currentSequences[sequenceIndex].entries.Length)
        {
            sequenceIndex++;
            entireIndex = 0;
            if (sequenceIndex >= currentSequences.Count)
            {
                sequencesEnded();
                return;
            }
        }
        runCurrentEntire();
    }

    void sequencesEnded()
    {
        Event_System.sequenceEnds();
        objectsAndComponents.hide();
        if (onSequencesEnd == OnSequencesEnd.NextWave)
        {
            Debug.Log("Next Wave from dialog manager");
            Event_System.spawnNextWave();
        }
        else if (onSequencesEnd == OnSequencesEnd.EndLevel)
        {
            Debug.Log("Next Level");
            Event_System.loadNextLevel();
        }
        else if (onSequencesEnd == OnSequencesEnd.Custom)
        {
            if (customPostSequenceAction != null)
                customPostSequenceAction.Invoke();
        }
    }

    public void setCustomPostSequenceAction(System.Action action)
    {
        customPostSequenceAction = action;
    }

    [System.Serializable]
    public class ObjectsAndComponents
    {

        public ObjectsAndComponents(GameObject parent)
        {
            this.parent = parent;
            definedObjectsAndComponents();
        }

        GameObject parent;
        GameObject backgroundObj;
        GameObject subjectObj;
        GameObject dialogPanelObj;
        GameObject subjectNameObj;
        GameObject dialogObj;
        GameObject nextButtonObj;
        GameObject nextTextObj;
        public Image backgroundImage;
        public Image subjectImage;
        public TextMeshProUGUI subjectName;
        public TextMeshProUGUI dialog;


        void definedObjectsAndComponents()
        {
            AssignGameObject(ref this.backgroundObj, "background");
            AssignGameObject(ref this.subjectObj, "subject");
            AssignGameObject(ref this.dialogPanelObj, "dialog_panel");
            AssignGameObject(ref this.subjectNameObj, this.dialogPanelObj, "subject_name");
            AssignGameObject(ref this.dialogObj, this.dialogPanelObj, "dialog");
            AssignGameObject(ref this.nextButtonObj, this.dialogPanelObj, "next_button");
            AssignGameObject(ref this.nextTextObj, this.nextButtonObj, "next_text");

            this.backgroundImage = this.backgroundObj.GetComponent<Image>();
            this.subjectImage = this.subjectObj.GetComponent<Image>();
            this.subjectName = this.subjectNameObj.GetComponent<TextMeshProUGUI>();
            this.dialog = this.dialogObj.GetComponent<TextMeshProUGUI>();
        }

        public void hide()
        {
            if(this.backgroundObj)
                this.backgroundObj.SetActive(false);
            if(this.subjectObj)
                this.subjectObj.SetActive(false);
             if(this.dialogPanelObj)
                this.dialogPanelObj.SetActive(false);
        }

        public void show()
        {
            this.backgroundObj.SetActive(true);
            this.subjectObj.SetActive(true);
            this.dialogPanelObj.SetActive(true);
        }

        void AssignGameObject(ref GameObject field, string path)
        {
            field = parent.transform.Find(path)?.gameObject;
        }

        void AssignGameObject(ref GameObject field, GameObject parent, string path)
        {
            field = parent.transform.Find(path)?.gameObject;
        }
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
    public int waveNumberIndex;
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