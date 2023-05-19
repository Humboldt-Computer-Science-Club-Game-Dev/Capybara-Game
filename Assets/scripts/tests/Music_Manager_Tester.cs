using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager_Tester : MonoBehaviour
{

    public bool runTest1 = false;
    public bool runTest2 = false;
    public bool runTest3 = false;
    public bool runTest4 = false;
    public bool runTest5 = false;
    public bool runTest6 = false;
    public bool runTest2_1 = false;
    public bool runTest2_2 = false;
    public bool runTest2_3 = false;
    public bool runTest2_4 = false;
    public bool runTest2_5 = false;
    public bool runTest2_6 = false;
    public bool miscTest = false;

    float timer = 0;
    float maxTime = 4f;
    bool played = false;


    float timer2 = 0;
    float maxTime2 = 5f;
    bool played2 = false;
    void Start()
    {
        if (miscTest) miscStartFunc();
        if (runTest1) test1Start();
        if (runTest2) test2Start();
        if (runTest3) test3Start();
        if (runTest4) test4Start();
        if (runTest5) test5Start();
        if (runTest6) test6Start();
        if (runTest2_1) test2_1Start();
        if (runTest2_2) test2_2Start();
        if (runTest2_3) test2_3Start();
        if (runTest2_4) test2_4Start();
        if (runTest2_5) test2_5Start();
        if (runTest2_6) test2_6Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (miscTest) miscUpdateFunc();
        if (runTest1) test1Update();
        if (runTest2) test2Update();
        if (runTest3) test3Update();
        if (runTest4) test4Update();
        if (runTest5) test5Update();
        if (runTest6) test6Update();
        if (runTest2_1) test2_1Update();
        if (runTest2_2) test2_2Update();
        if (runTest2_3) test2_3Update();
        if (runTest2_4) test2_4Update();
        if (runTest2_5) test2_5Update();
        if (runTest2_6) test2_6Update();
    }

    /* 
        **Sequential Play Test:** Verify that when two songs are triggered 
        sequentially, the second song starts playing after the first one ends.
    */
    void test1Start()
    {

    }
    void test1Update()
    {

    }

    /* 
        **Transition Effect Test:** Ensure that songs with the 
        transitionPlay property set to true transition from volume 0 to the 
        desired volume over the specified transitionDuration. If the 
        transitionDuration is greater then the the remaining time on the song, 
        the song will loop in order to transition down for desired amount of 
        time. 
    */
    void test2Start()
    {

    }
    void test2Update()
    {

    }

    /* 
        **Sequential Transition Test:** When two songs are triggered 
        sequentially and the second song has the transitionPlay property set 
        to true, ensure that the second song transitions in after the first 
        one. Also, the second song should transition out at the end. The first 
        song should play normally without any volume transitions if 
        transitionPlay is not set to true. 
    */
    void test3Start()
    {

    }
    void test3Update()
    {

    }

    /* 
        **Single Song Transition Test:** When a song has the transitionPlay 
        property set to true, it should transition from volume 0 to the 
        desired volume at the beginning. As the song nears the end, it should 
        transition from the current volume to 0. 
    */
    void test4Start()
    {

    }
    void test4Update()
    {

    }

    /* 
        **Force Play Test:** When forcePlay is enabled, the currently 
        playing song should be placed next in the queue. The new song should 
        start playing immediately. 
    */
    void test5Start()
    {

    }
    void test5Update()
    {

    }

    /* 
        **Force and Transition Play Test:** When both forcePlay and 
        transitionPlay are enabled, the currently playing song should smoothly 
        transition to volume 0 over the duration of transitionDuration The old 
        song should also be moved to the front of the queue. The new song that 
        gets played should transition from volume 0 to the desired volume. 
    */
    void test6Start()
    {

    }
    void test6Update()
    {

    }

    /* 
        **Pause Test:** When the pause method is called, the currently 
        playing music should transition to volume 0 if pause Transition is set 
        to true. The music should then be paused via the audio source. 
    */
    void test2_1Start()
    {

    }
    void test2_1Update()
    {

    }

    /* 
        **Stop Test:** When the stop method is called, the currently 
        playing music should transition to volume 0 if pause Transition is set 
        to true. The music should then be stopped via the audio source. 
    */
    void test2_2Start()
    {

    }
    void test2_2Update()
    {

    }

    /* 
        **Resume Test:** When resume method is called, audio source.play is 
        called and the audio will transition to correct volume when pause 
        transition is true. 
    */
    void test2_3Start()
    {

    }
    void test2_3Update()
    {

    }

    /* 
        **Standalone Clear Queue Test:** Clears the queue and leaves the 
        currently playing music alone. Music that is on loop may have already 
        began transitioning out before this method is called. This method will 
        not halt that transition. 
    */
    void test2_4Start()
    {

    }
    void test2_4Update()
    {

    }

    /* 
        **Clear Queue in Settings Test 1:** When play music is called, and 
        clear que is called and also force play, the current song will 
        transition or not transition to the newly passed in song. The current 
        song is not added to beginning of queue and existing queue will be 
        removed. 
    */
    void test2_5Start()
    {

    }
    void test2_5Update()
    {

    }

    /* 
        **Clear Queue in Settings Test 2:** When play music is called and 
        clearQueue is true and force play is not true, the same thing as test 
        1 will happen but there will be a warning log explaining what has 
        happened just in case the result is not clear to the user. 
    */
    void test2_6Start()
    {

    }
    void test2_6Update()
    {

    }
    void miscStartFunc()
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = false;
        musicSettings.transitionPlay = false;
        musicSettings.pauseTransitions = false;
        musicSettings.transitionDuration = 3f;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        /* 
            It is required that you create a Music Settings object when 
            you want to change the settings. 
        */
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = false;
        musicSettings2.pauseTransitions = true;
        musicSettings2.transitionPlay = true;
        musicSettings2.transitionDuration = 3f;
        musicSettings2.loop = false;
        musicSettings2.volume = 1;
        musicSettings2.pitch = 1;


        Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);

        Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);

        Music_Manager.PlayMusic("The_geese_kind_4", musicSettings2);
        Music_Manager.PlayMusic("cappy_defense_force_1", musicSettings2);

        Music_Manager.PlayMusic("The_geese_kind_3", musicSettings);

        Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    }
    void miscUpdateFunc()
    {
        timer += Time.deltaTime;
        if (timer > maxTime && !played)
        {
            MusicSettings musicSettings2 = new MusicSettings();
            musicSettings2.forcePlay = true;
            musicSettings2.transitionPlay = true;
            musicSettings2.transitionDuration = 20f;
            musicSettings2.loop = true;
            musicSettings2.volume = 1;
            musicSettings2.clearQueue = false;
            musicSettings2.pitch = 1;
            Music_Manager.PlayMusic("The_geese_kind_1", musicSettings2);
            /* Music_Manager.PauseMusic(); */
            /* Music_Manager.ClearQueue(); */
            played = true;
            Debug.Log("Inner Action");
        }

        if (played) timer2 += Time.deltaTime;
        if (timer2 > maxTime2 && !played2)
        {
            MusicSettings musicSettings = new MusicSettings();
            musicSettings.forcePlay = true;
            musicSettings.transitionPlay = true;
            musicSettings.transitionDuration = 2f;
            musicSettings.loop = true;
            musicSettings.volume = 1;
            musicSettings.pitch = 1;
            Music_Manager.PlayMusic("The_geese_kind_4", musicSettings);
            /* Music_Manager.ResumeMusic(); */
            played2 = true;
            Debug.Log("Inner Action 2");
        }
    }
}
