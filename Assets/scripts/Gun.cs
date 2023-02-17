using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gun : MonoBehaviour
{
    public enum sideOptions{
        player,
        enemy,
        undecided
    }
    private sideOptions side;
    private  GameObject bullet;
    public float bulletCooldown = 0.125f;
    private float bulletCooldownTimer = 0;
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
        bulletCooldownTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && side == sideOptions.player && bulletCooldownTimer >= bulletCooldown)
        {
            bulletCooldownTimer = 0;
            shoot();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) && side == sideOptions.player){
            bulletCooldownTimer = bulletCooldown;
        }
    }

    public void shoot(){
        GameObject shotBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        shotBullet.GetComponent<Bullet>().setSide(side);
        Debug.Log("Shoot");
    }
}
