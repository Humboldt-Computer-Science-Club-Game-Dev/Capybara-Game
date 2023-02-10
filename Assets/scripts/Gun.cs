using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gun : MonoBehaviour
{
    enum sideOptions{
        player,
        enemy,
        undecided
    }
    sideOptions side;
    public GameObject bullet;
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
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && side == sideOptions.player)
        {
            Debug.Log("Shoot");
            GameObject shotBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    public void shoot(){
        Debug.Log("Shoot");
    }
}
