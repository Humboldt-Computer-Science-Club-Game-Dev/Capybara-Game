using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.1f;
    /* TODO: Find a way to share this enum between the Gun script so that this enum only needs to be defined once */
    private enum sideOptions{
        player,
        enemy,
        undecided
    }
    private  CircleCollider2D circleCollider;
    sideOptions side;
    void Awake(){
        circleCollider = GetComponent<CircleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach(Collider2D hit in hits){
            if (hit == circleCollider) continue;
            ColliderDistance2D colliderDistance = hit.Distance(circleCollider);
            if(colliderDistance.isOverlapped  && hit.gameObject.tag == "Bullet_Bound"){
                Destroy(this.gameObject);
            }
        }
    }
    void FixedUpdate()
    {
        if(side == sideOptions.player){
            // Move the bullet to the right
            this.transform.position = new Vector3(this.transform.position.x + speed, this.transform.position.y, this.transform.position.z);
        }
        else if(side == sideOptions.enemy){
            // Move the bullet to the left
            this.transform.position = new Vector3(this.transform.position.x - 0.1f, this.transform.position.y, this.transform.position.z);
        }
        
    }

    void setSide(sideOptions /* `side` is a variable that is used to determine which side the bullet is
    on. */
    side){
        this.side = side;
    }
}
