using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_System : MonoBehaviour
{
    public delegate void damageTaken(int damage, string to);
    public static event damageTaken onDamageTaken;
    
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
        onDamageTaken(damage, to);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
