using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This script is needed various objects active. These an object that is 
    inactive can't set itself active so this script is like a third party 
    that will find the game object in question and set it active.
*/
public class set_needed_active : MonoBehaviour
{
    static Health_UI_Player healthUIPlayer;
    static Player_UI_Death playerUIDeath;
    public static void setHealthUIPlayerActive(){
        healthUIPlayer = (Health_UI_Player)FindObjectOfType(typeof(Health_UI_Player), true);
        healthUIPlayer.gameObject.SetActive(true);
    }
    public static void setPlayerUIDeathActive(){
        playerUIDeath = (Player_UI_Death)FindObjectOfType(typeof(Player_UI_Death), true);
        playerUIDeath.gameObject.SetActive(true);
    }
}
