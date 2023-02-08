using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death_Anim : MonoBehaviour
{
    enum deathState {alive, toCenter, rotate, falling, dead};
    private deathState state;
    private Vector3 center = new Vector3(0, 0, 0);
    private float minDistance = 0.01f;

    public float maxRotationTimer = 2f;
    private float rotationTime = 0;

    private Quaternion startRotation;
    private Vector2 outOfBounds1;
    private Vector2 outOfBounds2;
    private Vector2 outOfBounds3;

    private player player;

    // Start is called before the first frame update
    void Start()
    {
        state = deathState.alive;
        startRotation = new Quaternion(0, 0, 0, 10);
        outOfBounds1 = new Vector2(0, -10);
        outOfBounds2 = new Vector2(0, -20);
        outOfBounds3 = new Vector2(0, -40);
        player = GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void play(){
        state = deathState.toCenter;
    }
    void FixedUpdate(){
        if(state == deathState.toCenter)
        {
            // Move player to center of screen
            // Once player is in center, change state to rotate
            this.transform.position = Vector2.MoveTowards(this.transform.position, center, 0.1f);
            if(Vector3.Distance(this.transform.position, center) < minDistance)
            {
                state = deathState.rotate;
            }

        }
        else if(state == deathState.rotate)
        {
            this.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 180), ((rotationTime) / (maxRotationTimer * 50)));
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
            this.transform.position = Vector2.MoveTowards(this.transform.position, outOfBounds1, 0.1f);
            if(Vector3.Distance(this.transform.position, outOfBounds1) < minDistance)
            {
                state = deathState.dead;
                player.playerDeathAnimDone();
            }
            // After player has fallen, call event for game over.
            // Game over event should wrap up the level.
            // The level manager that calls the game over event will reload the current level on the line below.
        }
    }
    
}
