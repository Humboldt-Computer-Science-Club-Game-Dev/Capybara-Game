using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event_System : MonoBehaviour
{
    public delegate void damageTaken(int damage, string to);
    public static event damageTaken onDamageTaken;

    public delegate void death(string to);
    public static event death onDeath;

    public delegate void gameOverDel();
    public static event gameOverDel onGameOver;
    
    private void Awake(){
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /* 
        I have no dam clue why but events like onDamageTaken canot be called 
        outside of this script file ¯\_(ツ)_/¯ 
    */
    public static void takeDamage(int damage, string to){
        if(onDamageTaken != null) onDamageTaken(damage, to);
    }
    public static void die(string to){
        if(onDeath != null) onDeath(to);
    }
    public static void gameOver(){
        if(onGameOver != null) onGameOver();
        SceneManager.LoadScene("AnthonysProtoype");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
