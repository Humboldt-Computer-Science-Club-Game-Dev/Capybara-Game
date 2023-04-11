using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Note because this game object this script is attached starts off as inactive. 
    This script dose not have a place where it is set to active. This game object
    is set to active by the player script upon player death.
*/
public class Player_UI_Death : MonoBehaviour
{
    [SerializeField, Tooltip("Length of time to show the Cappy Died Screen")]
    public float timeToBeShown = 3f;
    bool isShown = false;
    float timeShown = 0f;
    void Update()
    {
        if(isShown) if(hasShownLongEnough()) deactivate();
    }
    public void beginDeathText(){
        isShown = true;
    }
    bool hasShownLongEnough(){
        timeShown += Time.deltaTime;
        return timeShown >= timeToBeShown;
    }
    void deactivate(){
        isShown = false;
        timeShown = 0f;
        this.gameObject.SetActive(false);
        GameObject.Find("capy").GetComponent<player>().playerDeathUIShownEnough();  
    }
}
