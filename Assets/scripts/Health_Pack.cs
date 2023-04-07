using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pack : MonoBehaviour
{
    public int healAmount = 1;
    [SerializeField, Tooltip("Set gravity seed greater than 0 to make the health pack move towards the gravity target")]
    public float gravitySpeed = 0f;
    [SerializeField, Tooltip("Enter the name of the object in scene view that the health pack will move towards")]
    public string gravityTarget = "capy";
    [SerializeField, Tooltip("Enter the tag of the object that the health pack will heal")]
    public string gravityTargetTag = "Player";
    private BoxCollider2D boxCollider;
    private Transform playerLocation;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        playerLocation = GameObject.Find(gravityTarget).GetComponent<Transform>();
    }

    public float lifetime = 10f;
    float timer = 0f;

    void Update()
    {
        deSpawnAfterLifetime();
        healOnTargetCollision();
    }

    // Just in case you want to change the gravity target at runtime
    public void setGravityTarget(string target){
        gravityTarget = target;
    }
    // Just in case you want to change the gravity target tag at runtime
    public void setGravityTargetTag(string targetTag){
        gravityTargetTag = targetTag;
    }

    void FixedUpdate(){
        // Moves the health pack towards the gravity target at the speed of gravitySpeed
        transform.position = Vector2.MoveTowards(transform.position, playerLocation.position, gravitySpeed * Time.deltaTime);
    }

    void deSpawnAfterLifetime(){
        timer += Time.deltaTime;
        if(timer > lifetime){
            Destroy(this.gameObject);
        }
    }

    void healOnTargetCollision(){
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        foreach (Collider2D hit in hits){
        	if (hit == boxCollider)
        	continue;

           if(hit.gameObject.tag == gravityTargetTag){
              hit.gameObject.GetComponent<player>().healPlayer(healAmount);
               Destroy(this.gameObject);
           }
        }
    }
}
