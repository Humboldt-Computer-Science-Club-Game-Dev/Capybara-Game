using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Brain : MonoBehaviour
{

    enum Movement {positioning, oscillating};
    private Movement movementState = Movement.positioning;
    private PolygonCollider2D polygonCollider;
    private Camera cam;

    public Vector2 topRight;
    public Vector2 bottomLeft;

    public float halfWidth;
    public float halfHeight;

    public int designatedRestBound = 1;

    private void Awake()
    {      
        polygonCollider = GetComponent<PolygonCollider2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        halfWidth = this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        halfHeight = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0,0));
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
        handleOscillating();
    }

    void handleRestBoundCollision(Collider2D hit, ColliderDistance2D colliderDistance){
        if(colliderDistance.isOverlapped && hit.gameObject.tag == "Enemy_Rest_Bound" && movementState == Movement.positioning){
            Enemy_Rest_Bound restBound = hit.gameObject.GetComponent<Enemy_Rest_Bound>();
            if(restBound.restBound != designatedRestBound) return;
            movementState = Movement.oscillating;
            GameObject enemyMovementSpace = GameObject.Find("enemy_movement_space");
            this.gameObject.transform.SetParent(enemyMovementSpace.transform, true);
        }
    }
    void handleOscillating(){
        if(movementState != Movement.oscillating) return;
    }
}
