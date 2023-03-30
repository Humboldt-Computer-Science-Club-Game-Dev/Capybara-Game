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
    
    // Start is called before the first frame update
    public void setAsPlayer(){
        side = sideOptions.player;
        
    }
    public void setAsEnemy(){
        side = sideOptions.enemy;
    }
    void Start()
    {
        bullet = (GameObject)Resources.Load("prefabs/bullet", typeof(GameObject));
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void shoot(){
        if(bullet == null) return;
        GameObject shotBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        shotBullet.GetComponent<Bullet>().setSide(side);
    }
}
