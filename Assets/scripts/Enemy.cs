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
    private List<int> IDsThatShotMe;
    private Health health;
    private int id;
    private ColliderDistance2D pushPlayerDistanceCalculationObject;
    private Enemy_Death_Anim enemyDeathAnim;
    const string HEALTH_PACK_PREFAB_PATH = "prefabs/health_pack";

    private void Awake()
    {      
        getComponents();
        IDsThatShotMe = new List<int>();
    }

    void Start()
    {
        assignEvents();
        //Bug where an enemy death counts as two in wave manager is caused by the fact that if one enemy
        id = Random_Number_Generator.generateUnique4DigitNumber();
        enemyDeathAnim.receiveID(id);
    }

    void Update()
    {
        handleCollisions();
    }
    void getComponents(){
        polygonCollider = GetComponent<PolygonCollider2D>();
        health = GetComponent<Health>();
        enemyDeathAnim = GetComponent<Enemy_Death_Anim>();
    }
    void assignEvents(){
        Event_System.onDamageTaken += takeDamageAndHandleDeathCall;
        Event_System.onDeath += onDeath;
    }
    void handleCollisions(){
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        int hits = Physics2D.OverlapCollider(polygonCollider, filter, results);
        foreach (Collider2D hit in results)
        {
        	if (hit == polygonCollider) continue; // Ignore collision with self
        	
            // Get the distance between the two colliders
        	ColliderDistance2D colliderDistance = hit.Distance(polygonCollider);

            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet") handleBulletInMe(hit.gameObject.GetComponent<Bullet>());

            if(colliderDistance.isOverlapped && hit.gameObject.tag == "Player" && !isMeleeAttacking){
                pushPlayerDistanceCalculationObject = colliderDistance;
                player = hit.gameObject.GetComponent<CharacterController2D>();

                isMeleeAttacking = true;
            }
        }
        handleMeleeAttack();
    }
    void handleMeleeAttack(){
        if(!isMeleeAttacking) return;
        if(gracePeriodUp){ //will get run when play first enters melee range
                if(!player.GetComponent<Health>().isDead()) 
                    player.GetComponent<CharacterController2D>().getPushed(-4 * (pushPlayerDistanceCalculationObject.pointA - pushPlayerDistanceCalculationObject.pointB).normalized);
                Event_System.takeDamage(meleeDamage, "player");
        }
        gracePeriodUp = false; // prevents the above code from running every frame
        meleeDamageGracePeriodTimer += Time.deltaTime;

        if(meleeDamageGracePeriodTimer >= meleeDamageGracePeriod){ // After some time has passed, the enemy will be able to meele attack again
            isMeleeAttacking = false;
            gracePeriodUp = true;
            meleeDamageGracePeriodTimer = 0f;
        }
    
    }


   /*  If the bullet's ID is not in the list of IDs of bullets that have hit me, add it to the list and
    call the getShot() function */
    void handleBulletInMe(Bullet bullet){
        if(hasAlreadygetShotByThisBullet(bullet.id) || bullet.isEnemyBullet()) return;
        IDsThatShotMe.Add(bullet.id);
        getShot();
        Destroy(bullet.gameObject);
        
    }
    bool hasAlreadygetShotByThisBullet(int bulletID){
        return IDsThatShotMe.Contains(bulletID);
    }
    
    void getShot(){
        Event_System.takeDamage(1, "enemy" + id);
    }
    void takeDamageAndHandleDeathCall(int damage, string to){
        if(!(to == "enemy" + id)) return;
        
        health.takeDamage(damage);

        
        /* This is a bit out of place but its the only to check for death */
        if(health.isDead()){
            if(Random_Number_Generator.fiftyFifty() == 1) spawnHealthPack();
            Event_System.onDamageTaken -= takeDamageAndHandleDeathCall;
            this.gameObject.transform.SetParent(null, true);
            enemyDeathAnim.playDeathAnim();
        }
    }
    void spawnHealthPack(){
        GameObject healthPack = Instantiate((GameObject)Resources.Load(HEALTH_PACK_PREFAB_PATH, typeof(GameObject)), transform.position, transform.rotation);

        /* Setting the parent of the health pack to the environment_space object 
        so that it travels to the left and gives player the chance to pick it up */
        healthPack.transform.SetParent(GameObject.Find("enviroment_space").transform, true);
    }
    void onDeath(string to){
        if(!(to == ("enemy" + id))) return;

        Event_System.onDeath -= onDeath;
    }

    public void destroyEnemy(){
        Destroy(gameObject);
    }

    public int getID(){
        return id;
    }
}
