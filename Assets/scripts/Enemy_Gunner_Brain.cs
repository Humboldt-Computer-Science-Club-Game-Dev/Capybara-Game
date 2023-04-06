using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gunner_Brain : MonoBehaviour
{
    float initialShootOffsetTime;
    bool firstShot = false;
    float shootTimer = 0;
    public float shootCooldown = 2f;
    public float maxRandomInitialShotOffset = 4f;
    Gun gun;
    Health health;

    void Start()
    {
        getComponents();
        initialShootOffsetTime = Random_Number_Generator.randomPositiveFloatUnder(maxRandomInitialShotOffset);
        gun.setAsEnemy();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        /* 
            There is a difference between initial shot and 
            post initial shot in order to make sure shots are 
            not all fired at the same time. This is achieved 
            by adding a random offset to the first shot. And 
            during the remaining shots, the enemy will shoot 
            every shootCooldown seconds. 
        */
        handleInitialShot();
        handleShooting();
    }
    void getComponents(){
        health = GetComponent<Health>();
        gun = GetComponent<Gun>();
    }

    // If the enemy has not shot yet, it will shoot after a random amount of time
    void handleInitialShot(){
        if(firstShot) return;
        if(shootTimer >= initialShootOffsetTime){
            firstShot = true;
            shootTimer = 0;
            gun.shoot();
        }
        
    }

    // If the enemy has shot, it will shoot every shootCooldown seconds
    void handleShooting(){
        if(!firstShot) return;
        if(shootTimer >= shootCooldown && !health.isDead()){
            shootTimer = 0;
            gun.shoot();
        }
    }
}
