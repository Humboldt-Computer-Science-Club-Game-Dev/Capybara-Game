using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gunner_Brain : MonoBehaviour
{
    int shotOffsetTime;
    bool firstShot = false;
    float shootTimer = 0;
    public float shootCooldown = 2f;
    Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        float mappedNum = Random_Number_Generator.random1DigitNumber() / 2.0f;
        shotOffsetTime = (int)Mathf.Clamp(mappedNum, 0, 4);
        gun = GetComponent<Gun>();
        gun.setAsEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        handleInitialShot();
        handleShooting();
    }
    void handleInitialShot(){
        if(!firstShot){
            if(shootTimer >= shotOffsetTime){
                firstShot = true;
                shootTimer = 0;
                gun.shoot();
            }
        }
    }
    void handleShooting(){
        if(!firstShot) return;
        if(shootTimer >= shootCooldown){
            shootTimer = 0;
            gun.shoot();
        }
    }
}
