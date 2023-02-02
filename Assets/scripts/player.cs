using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        initializeHealth();
        initializeUI();
    }

    void initializeUI(){
        Event_System.onDamageTaken += updateLifeUI;
    }

    void initializeHealth(){
        playerHealth = GetComponent<Health>();
        playerHealth.to = "player";
        Event_System.onDamageTaken += playerHealth.takeDamage;
    }

    void updateLifeUI(int damage, string to){
        if(to == "player"){
            Health_UI_Player.updateLife(playerHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
