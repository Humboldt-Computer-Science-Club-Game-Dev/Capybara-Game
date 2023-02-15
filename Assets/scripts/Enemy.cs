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

    private Health health;
    private int id;

    private void Awake()
    {      
        polygonCollider = GetComponent<PolygonCollider2D>();
        shotByIDs = new List<int>();
        health = GetComponent<Health>();
        //assign id to a 10 digit random number
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Event_System.onDamageTaken += takeDamage;
        id = 0;
    }
    void assignRandomID(){
        Random.InitState(System.DateTime.Now.Millisecond);
        id = (int)Random.Range(1000000000, 9999999999);
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
            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet") bulletInMe(hit.gameObject.GetComponent<Bullet>());
                

            
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
    /// <summary>
    /// If the bullet's ID is not in the list of IDs of bullets that have hit me, add it to the list and
    /// call the beenShot() function
    /// </summary>
    /// <param name="Bullet">The bullet that hit the enemy</param>
    void bulletInMe(Bullet bullet){
        if(!shotByIDs.Contains(bullet.id)){
            shotByIDs.Add(bullet.id);
            beenShot();
        }
    }
    // This is a cockamani event system. We need to find a better way to do this after this prototype is done.
    void beenShot(){
        assignRandomID();
        Event_System.takeDamage(1, "enemy" + id);
    }
    void takeDamage(int damage, string to){
        //When id = 0 the ID has not been assigned yet. The IDs can't be assigned all at once that will make all there IDs the same.
        // Don't ask me why the Random.Range() works this way. I don't know. ¯\_(ツ)_/¯
        if(!(to == "enemy" + id) || id == 0) return;
        Debug.Log("Enemy take damage of enemy" + id + " vs " + to);
        health.takeDamage(damage);
    }
}
