using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death_Anim : MonoBehaviour
{
    public float maxRotationTimer = 2f;
    public float toCenterSpeed = 0.1f;
    public float fallSpeed = 0.1f;
    enum deathState {alive, toCenter, rotate, falling, dead};
    deathState state;
    Vector3 center = new Vector3(0, 0, 0);
    float minDistance = 0.01f;
    float rotationTime = 0;
    Vector2 outOfBounds;
    player player;

    void Start()
    {
        state = deathState.alive;
        outOfBounds = new Vector2(0, -10);
        player = GetComponent<player>();
    }

    // Starts the death animation
    public void play(){
        state = deathState.toCenter;
    }
    void FixedUpdate(){
        if(state == deathState.toCenter)
        {
            // Move player to center of screen
            // Once player is in center, change state to rotate
            this.transform.position = Vector2.MoveTowards(this.transform.position, center, toCenterSpeed);
            if(Vector3.Distance(this.transform.position, center) < minDistance) state = deathState.rotate;
        }
        else if(state == deathState.rotate)
        {
            // Rotate player 180 degrees in maxRotationTimer seconds
            this.transform.rotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, 0), 
                Quaternion.Euler(0, 0, 180), 
                ((rotationTime) / (maxRotationTimer * 50)));

            rotationTime += 1;

            // Once player is rotated, change state to falling
            if(rotationTime > (maxRotationTimer * 50))
            {
                rotationTime = 0;
                state = deathState.falling;
            }
            
        }
        else if(state == deathState.falling)
        {
            // Move player off screen at the speed of fallSpeed
            this.transform.position = Vector2.MoveTowards(this.transform.position, outOfBounds, fallSpeed);

            if(Vector3.Distance(this.transform.position, outOfBounds) < minDistance)
            {
                state = deathState.dead;

                // Tell player that death animation is done so that it can do the next steps in player death
                player.playerDeathAnimDone();
            }
        }
    }
    
}
