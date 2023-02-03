using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Health_UI_Player health_UI_player;
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        initializeHealth();
        initializeUI();
    }

    void initializeUI(){
        Event_System.onDamageTaken += updateLifeUI;
        health_UI_player = GameObject.Find("player_health").GetComponent<Health_UI_Player>();
    }

    void initializeHealth(){
        playerHealth = GetComponent<Health>();
        Event_System.onDamageTaken += damageTaken;
    }

    void damageTaken(int damage, string to){
        if(to == "player"){
            playerHealth.takeDamage(damage);
        }
    }

    void updateLifeUI(int damage, string to){
        if(to == "player"){
            health_UI_player.updateLife(playerHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
