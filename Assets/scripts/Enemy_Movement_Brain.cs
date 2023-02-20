using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Brain : MonoBehaviour
{

    enum Movement {positioning, oscillating};
    private Movement movementState = Movement.positioning;
    private PolygonCollider2D polygonCollider;

    private void Awake()
    {      
        polygonCollider = GetComponent<PolygonCollider2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Enemy_Rest_Bound" && movementState == Movement.positioning){
                Debug.Log("Enemy_Rest_Bound");
            }
        }
    }
}
