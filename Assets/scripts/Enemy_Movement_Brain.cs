using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Factor out enemy roaming logic into Enemy_Roaming_Brain

public class Enemy_Movement_Brain : MonoBehaviour
{
    public enum Movement {positioning, oscillating, roaming};
    enum OscillationDirection {up, down};

    public int oscillationSpeed = 1;
    private Movement movementState = Movement.positioning;
    private OscillationDirection oscillationDirection = OscillationDirection.up;
    private PolygonCollider2D polygonCollider;
    private Camera cam;

    Vector2 topRight;
    Vector2 bottomLeft;

    float halfWidth;
    float halfHeight;

    public int designatedRestBound = 1;

    public Health health;
    public float roamCooldown = 20f;
    public float roamCooldownTimer = 0f;
    public Enemy_Roaming_Brain roamingBrain;
    public bool roamer = false;
    private void Awake()
    {      
        polygonCollider = GetComponent<PolygonCollider2D>();
        roamCooldownTimer = Random_Number_Generator.randomPositiveFloatUnder(roamCooldown);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        halfWidth = this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        halfHeight = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0,0));
        health = GetComponent<Health>();
        Enemy_Roaming_Brain roamingBrainBuffer = GetComponent<Enemy_Roaming_Brain>();
        if(roamingBrainBuffer != null){
            roamer = true;
            roamingBrain = roamingBrainBuffer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        /* transform.position, polygonCollider.offset, 0 */
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
