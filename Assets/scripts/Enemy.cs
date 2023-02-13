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

    private CharacterController2D player;

    private PolygonCollider2D polygonCollider;
    private List<int> shotByIDs;

    private void Awake()
    {      
        polygonCollider = GetComponent<PolygonCollider2D>();
        shotByIDs = new List<int>();
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
                player = hit.gameObject.GetComponent<CharacterController2D>();

                //Meant to push player back when not dead.
                // The isMeleeAttacking = true would kill the player before they are pushed if it went for that fact that the player killing logic
                // is implemented a few lines below this.
                // TLDR: This is a bad implementation of pushing the player back.
                if(!player.playerHealth.isDead()) player.isPushed = true;
            }
            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet"){
                Debug.Log("Enemy hit by bullet");
            }
        }
        if(isMeleeAttacking){
            if(gracePeriodUp){
                /* player.GetComponent<Health>().takeDamage(meleeDamage); */
                Event_System.takeDamage(meleeDamage, "player");
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
    void bulletInMe(){
        Debug.Log("Bullet in me");
    }
    void beenShot(){
        Debug.Log("Enemy been shot");
    }
}
