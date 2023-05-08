using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager_Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicSettings musicSettings = new MusicSettings();
        musicSettings.forcePlay = false;
        musicSettings.transitionPlay = false;
        musicSettings.transitionDuration = 0;
        musicSettings.loop = true;
        musicSettings.volume = 1;
        musicSettings.pitch = 1;
        Music_Manager.PlayMusic("cappy_defense_force_2", musicSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
