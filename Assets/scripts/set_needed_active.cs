using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_needed_active : MonoBehaviour
{
    static Health_UI_Player healthUIPlayer;
    static Player_UI_Death playerUIDeath;
    // Start is called before the first frame update
    void Start()
    {
        setHealthUIPlayerActive();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setHealthUIPlayerActive(){
        healthUIPlayer = (Health_UI_Player)FindObjectOfType(typeof(Health_UI_Player), true);
        healthUIPlayer.gameObject.SetActive(true);
    }
    public static void setPlayerUIDeathActive(){
        playerUIDeath = (Player_UI_Death)FindObjectOfType(typeof(Player_UI_Death), true);
        playerUIDeath.gameObject.SetActive(true);
    }
}
