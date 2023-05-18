using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager_Tester : MonoBehaviour
{
    float timer = 0;
    float maxTime = 0.00f;
    bool played = false;
    // Start is called before the first frame update
    void Start()
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = false;
        musicSettings.transitionPlay = false;
        musicSettings.transitionDuration = 3f;
        musicSettings.loop = false;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;

        /* 
            It is required that you create a Music Settings object when you want to change the settings. 
        */
        MusicSettings musicSettings2 = new MusicSettings();
        musicSettings2.forcePlay = false;
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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTime && !played)
        {
            MusicSettings musicSettings2 = new MusicSettings();
            musicSettings2.forcePlay = true;
            musicSettings2.transitionPlay = true;
            musicSettings2.transitionDuration = 2f;
            musicSettings2.loop = true;
            musicSettings2.volume = 1;
            musicSettings2.pitch = 1;
            Music_Manager.PlayMusic("The_geese_kind_1", musicSettings2);
            played = true;
        }
    }
}
