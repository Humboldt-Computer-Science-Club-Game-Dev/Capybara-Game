using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gun : MonoBehaviour
{
    public enum sideOptions {
        player,
        enemy,
        undecided
    }
    private sideOptions side  = sideOptions.undecided;
    private  GameObject bullet;

    public void setAsPlayer(){
        side = sideOptions.player;
    }
    public void setAsEnemy(){
        side = sideOptions.enemy;
    }
    void Start()
    {
        // Load the bullet prefab
        bullet = (GameObject)Resources.Load("prefabs/bullet", typeof(GameObject));
    }

    public void shoot(){
        if(bullet == null) return;
        GameObject shotBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);

        // Set the side of the bullet
        //This is used to determine what direction the bullet should move and who it should apply damage to
        shotBullet.GetComponent<Bullet>().setSide(side);
    }
}
