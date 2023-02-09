using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI_Death : MonoBehaviour
{
    private bool isShown = false;
    private float timeShown = 0f;
    public float timeToBeShown = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isShown){
            timeShown += Time.deltaTime;
            if(timeShown >= timeToBeShown){
                isShown = false;
                timeShown = 0f;
                GameObject.Find("capy").GetComponent<player>().playerDeathUIShownEnough();
            }
        }
    }
    public void beginDeathText(){
        this.gameObject.SetActive(true);
        isShown = true;
    }
}
