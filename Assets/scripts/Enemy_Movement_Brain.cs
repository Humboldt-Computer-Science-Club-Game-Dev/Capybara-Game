using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Brain : MonoBehaviour
{
    [HideInInspector]
    public enum Movement {positioning, oscillating, roaming};
    public int designatedRestBound = 1;
    public int oscillationSpeed = 1;
    enum OscillationDirection {up, down};
    Movement movementState = Movement.positioning;
    OscillationDirection oscillationDirection = OscillationDirection.up;
    PolygonCollider2D polygonCollider;
    Camera cam;
    Vector2 topRight;
    Vector2 bottomLeft;
    float halfWidth;
    float halfHeight;
    Health health;
    Enemy_Roaming_Brain roamingBrain;
    
    void Start()
    {
        getComponents();
        assignBoundingInformation();
    }

    void Update()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        int hits = Physics2D.OverlapCollider(polygonCollider, filter, results);
        if(hits <= 0) return;
        foreach (Collider2D hit in results){
        	if (hit == polygonCollider)
        	continue;

        	ColliderDistance2D colliderDistance = hit.Distance(polygonCollider);

            handleRestBoundCollision(hit, colliderDistance);
        }
    }

    void FixedUpdate(){
        handleOscillating();
    }
    
    void getComponents(){
        polygonCollider = GetComponent<PolygonCollider2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        health = GetComponent<Health>();
        Enemy_Roaming_Brain roamingBrainBuffer = GetComponent<Enemy_Roaming_Brain>();
        if(roamingBrainBuffer != null) roamingBrain = roamingBrainBuffer;
    }

    void assignBoundingInformation(){
        halfWidth = this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        halfHeight = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0,0));
    }

    void handleRestBoundCollision(Collider2D hit, ColliderDistance2D colliderDistance){
        if(health.isDead()) return;
        if(colliderDistance.isOverlapped && hit.gameObject.tag == "Enemy_Rest_Bound" && movementState == Movement.positioning){
            Enemy_Rest_Bound restBound = hit.gameObject.GetComponent<Enemy_Rest_Bound>();
            if(restBound.restBound != designatedRestBound) return;
            movementState = Movement.oscillating;
            GameObject enemyMovementSpace = GameObject.Find("enemy_movement_space");
            this.gameObject.transform.SetParent(enemyMovementSpace.transform, true);
        }
    }
    //Warning, below code is messy
    void handleOscillating() {
        if(health.isDead()) {
            this.gameObject.transform.SetParent(null, true);
            return;
        }
        float movementSpeedFix = oscillationSpeed / 50f;
        if(movementState != Movement.oscillating) return;
        if(oscillationDirection == OscillationDirection.up){
             if(transform.position.y + halfHeight >= topRight.y)
                oscillationDirection = OscillationDirection.down; 
            else
                transform.position = new Vector2(transform.position.x, transform.position.y + movementSpeedFix);
        }
        else if(oscillationDirection == OscillationDirection.down){
            if(transform.position.y - halfHeight <= bottomLeft.y)
                oscillationDirection = OscillationDirection.up;
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - movementSpeedFix);
        }
    }

    //These methods are only ever called by the roaming brain
    public Movement getMovementState(){
        return movementState;
    }
    public void startRoaming(){
        movementState = Movement.roaming;
    }
    public bool isRoaming(){
        return movementState == Movement.roaming;
    }
    public void onFinishedRoaming(){
        movementState = Movement.oscillating;
    }
}
