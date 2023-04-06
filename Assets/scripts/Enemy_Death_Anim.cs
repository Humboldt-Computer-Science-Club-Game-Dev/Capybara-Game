using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death_Anim : MonoBehaviour
{
    enum deathState {alive, rotate, falling, dead};
    private deathState state;
    public float maxRotationTimer = 0.5f;
    public int offScreenAmountY = -10;
    public float fallSpeed = 0.5f;
    private float rotationTime = 0;
    private int enemyID;
    private Enemy enemy;
    void Start()
    {
        state = deathState.alive;
        enemy = GetComponent<Enemy>();
        enemyID = enemy.getID();
    }

    /* 
        Note: 
        Fixed update is called 50 times per second 
        so we multiply the max rotation timer by 50 so that 
        max rotation trimmer is in seconds 
    */
    void FixedUpdate()
    {
        if(state == deathState.rotate) handleEnemyRotation();
        else if(state == deathState.falling) handleEnemyFalling();
    }
    // Animate transform of enemy to move off screen
    void handleEnemyFalling(){
        makeEnemyFallOnce();
        if(this.gameObject.transform.position.y < offScreenAmountY){
            state = deathState.dead;
            Event_System.die("enemy" + enemyID);
            enemy.destroyEnemy();
        }
    }
    void makeEnemyFallOnce(){
        // Note that the enemy is upside down so to move the enemy down we move it up (relatively)
        this.gameObject.transform.Translate(Vector3.up * fallSpeed);
    }
    void handleEnemyRotation(){
        rotationTime += 1;
        rotateEnemyOnce();
        if(rotationTime > (maxRotationTimer * 50)){
            rotationTime = 0;
            state = deathState.falling;
        }
    }
    void rotateEnemyOnce(){
        this.gameObject.transform.rotation = Quaternion.Lerp(
            Quaternion.Euler(0, 0, 0), 
            Quaternion.Euler(0, 0, 180), 
            ((rotationTime) / (maxRotationTimer * 50))
        );
    }

    public void playDeathAnim(){
        state = deathState.rotate;
    }

    public void receiveEnemyID(int id){
        enemyID = id;
    }
}
