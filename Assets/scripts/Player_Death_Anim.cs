using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death_Anim : MonoBehaviour
{
    enum deathState {alive, toCenter, rotate, falling};
    private deathState state;
    // Start is called before the first frame update
    void Start()
    {
        state = deathState.alive;
    }

    // Update is called once per frame
    void Update()
    {

        if(state == deathState.toCenter)
        {

        }
        else if(state == deathState.rotate)
        {
        }
        else if(state == deathState.falling)
        {
            //After player has fallen, call event for game over
            //Game over event should wrap up the level.
            //The level manager that calls the game over event will reload the current level on the line below.
        }
    }
}
