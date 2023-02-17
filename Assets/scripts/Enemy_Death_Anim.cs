using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death_Anim : MonoBehaviour
{
    enum deathState {alive, rotate, falling, dead};
    private deathState state;

    public float maxRotationTimer = 0.5f;
    private float rotationTime = 0;
    private int enemyID;
    // Start is called before the first frame update
    void Start()
    {
        state = deathState.alive;
    }

    // Update is called once per frame
     void Update(){}
    void FixedUpdate()
    {
        if(state == deathState.rotate){
            rotationTime += 1;
            this.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 180), ((rotationTime) / (maxRotationTimer * 50)));
            if(rotationTime > (maxRotationTimer * 50)){
                rotationTime = 0;
                state = deathState.falling;
            }
        }
        else if(state == deathState.falling){
            // Animate transform of enemy to move off screen
            this.gameObject.transform.Translate(Vector3.up * 0.5f);
            if(this.gameObject.transform.position.y < -10){
                state = deathState.dead;
                Event_System.die("enemy" + enemyID);
            }
        }
    }

    public void playDeathAnim(){
        Debug.Log("Enemy Death Anim");
        this.gameObject.transform.SetParent(null, true);
        state = deathState.rotate;
    }

    public void receiveEnemyID(int id){
        enemyID = id;
    }
}
