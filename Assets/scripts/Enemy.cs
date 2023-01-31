using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int meleeDamage = 1;
    public int rangedDamage = 1;

    public float meleeDamageGracePeriod = 1f;
    private float meleeDamageGracePeriodTimer = 0f;

    private bool isMeleeAttacking = false;
    private bool gracePeriodUp = true;

    private GameObject player;

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
        foreach (Collider2D hit in results)
        {
        	if (hit == polygonCollider)
        	continue;

        	ColliderDistance2D colliderDistance = hit.Distance(polygonCollider);

            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Player" && !isMeleeAttacking){
                isMeleeAttacking = true;
                player = hit.gameObject;
                // handle melee damage on player damage
            }
        }
        if(isMeleeAttacking){
            if(gracePeriodUp){
                player.GetComponent<Health>().takeDamage(meleeDamage);
            }
            gracePeriodUp = false;
            meleeDamageGracePeriodTimer += Time.deltaTime;
            if(meleeDamageGracePeriodTimer >= meleeDamageGracePeriod){
                isMeleeAttacking = false;
                gracePeriodUp = true;
                meleeDamageGracePeriodTimer = 0f;
            }
        }
    }
}
