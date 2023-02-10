using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Health_UI_Player health_UI_player;
    private Health playerHealth;
    private CharacterController2D playerController;
    private Player_Death_Anim playerDeathAnim;
    private Player_UI_Death playerUIDeath;
    private Gun gun;
    
    // Start is called before the first frame update
    void Start()
    {
        initializeHealth();
        initializeUI();
        playerDeathAnim = GetComponent<Player_Death_Anim>();
        GameObject playerUIDeathGameObject = GameObject.Find("player_death_screen");
        playerUIDeath = playerUIDeathGameObject.GetComponent<Player_UI_Death>();
        playerUIDeathGameObject.SetActive(false);
        gun = GetComponent<Gun>();
        gun.setAsPlayer();
    }

    void initializeUI(){
        Event_System.onDamageTaken += updateLifeUI;
        health_UI_player = GameObject.Find("player_health").GetComponent<Health_UI_Player>();
    }

    void initializeHealth(){
        playerController = GetComponent<CharacterController2D>();
        playerHealth = GetComponent<Health>();
        Event_System.onDamageTaken += damageTaken;
        Event_System.onDeath += playerDeath;
    }

    void damageTaken(int damage, string to){
        if(playerHealth.isDead()) return;
        if(to == "player"){
            playerHealth.takeDamage(damage);
            if(playerHealth.isDead()){
                Event_System.die("player");
            }
        }
    }

    void playerDeath(string to){
        if(to == "player"){
            playerController.onDeath();
            playerDeathAnim.play();
        }
    }

    public void playerDeathAnimDone(){
        // Gets called when expected
        playerUIDeath.beginDeathText();
    }

    public void playerDeathUIShownEnough(){
        Event_System.gameOver();
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
