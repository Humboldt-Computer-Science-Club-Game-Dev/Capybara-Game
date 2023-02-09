using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    enum sideOptions{
        player,
        enemy
    }
    sideOptions side;
    // Start is called before the first frame update
    void setAsPlayer(){
        side = sideOptions.player;
    }
    void setAsEnemy(){
        side = sideOptions.enemy;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot(){
        Debug.Log("Shoot");
    }
}
