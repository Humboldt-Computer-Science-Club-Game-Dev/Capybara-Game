using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog_System : MonoBehaviour
{
    private static Dialog_System instance;
    public Sequence[] Sequences;

    List<Sequence> currentSequences = new List<Sequence>();

    public OnSequencesEnd onSequencesEnd;

    Wave_System wave_System;

    ObjectsAndComponents objectsAndComponents;
    int sequenceIndex;
    int entireIndex;

    AudioSource audioSource;

    Camera mainCamera;

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

        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        objectsAndComponents = new ObjectsAndComponents(this.gameObject);
        objectsAndComponents.hide();

        dialogEvent(PlayOnOptions.Start);


    }


    void Update()
    {

    }


    public static void dialogEvent(PlayOnOptions playOnOption)
    {
        instance.currentSequences.Clear();
        if (playOnOption == PlayOnOptions.Start || playOnOption == PlayOnOptions.EndOfAllWaves)
        {
            foreach (Sequence sequence in instance.Sequences)
            {
                if (sequence.playOn.playOnOption == playOnOption)
                    instance.currentSequences.Add(sequence);
            }
            instance.startSequences();
        }
        else if (playOnOption == PlayOnOptions.WaveNumber)
        {
            Debug.LogError("Can not pass in type PlayOnOptions whose value is PlayOnOptions.WaveNumber. Please pass in an int instead to the dialogEvent function if you want Dialog_System to play a dialog sequence based on the wave number.");
            return;
        }

    }

    public static void dialogEvent(int waveNumber)
    {
        instance.currentSequences.Clear();
        foreach (Sequence sequence in instance.Sequences)
        {
            if (sequence.playOn.playOnOption == PlayOnOptions.WaveNumber && sequence.playOn.waveNumber == waveNumber)
                instance.currentSequences.Add(sequence);
        }
        instance.startSequences();
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
        Debug.Log("Running new entire");
        displayCurrentEntire();
        playCurrentEntireVoice();
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
        Image spriteRenderer = objectsAndComponents.subjectImage;
        RectTransform imageRectTransform = spriteRenderer.rectTransform;
        float scalePercentage = 0.9f;

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
            imageRectTransform.Rotate(0, 180, 0);

            imageRectTransform.anchorMin = new Vector2(0, 0);
            imageRectTransform.anchorMax = new Vector2(0, 0);
            imageRectTransform.pivot = new Vector2(0, 0);
            imageRectTransform.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            imageRectTransform.Rotate(0, 180, 0);

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
        Debug.Log("sequences ended");
        objectsAndComponents.hide();
        instance.wave_System.spawnNextWave();
    }

    public static void setCustomPostSequenceAction(System.Action action)
    {

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
            this.backgroundObj.SetActive(false);
            this.subjectObj.SetActive(false);
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