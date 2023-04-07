using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float bulletCooldown = 0.125f;
    private Health_UI_Player health_UI_player;
    private Health playerHealth;
    private CharacterController2D playerController;
    private Player_Death_Anim playerDeathAnim;
    private Player_UI_Death playerUIDeath;
    private Gun gun;
    private float bulletCooldownTimer = 0;
    private List<int> shotByIDs;
    private BoxCollider2D boxCollider;
    
    void Start()
    {
        getComponents();
        shotByIDs = new List<int>();

        /* 
            Note, its vitally important that you initialize health before UI because 
            the player damage listeners need to to run before the ui event listeners. 
            This insures that the UI is updated after the health is updated 
        */
        initializeHealth();
        initializeUI();
        gun.setAsPlayer();
        
    }
    void Update()
    {
        handlePlayerShoot();
        handleGetShot();
    }
    void getComponents(){
        playerDeathAnim = GetComponent<Player_Death_Anim>();
        gun = GetComponent<Gun>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void initializeUI(){
        Event_System.onDamageTaken += updateLifeUI;
        health_UI_player = GameObject.Find("player_health").GetComponent<Health_UI_Player>();
    }

    void initializeHealth(){
        playerController = GetComponent<CharacterController2D>();
        playerHealth = GetComponent<Health>();
        Event_System.onDamageTaken += damageTaken;
        Event_System.onDeath += playerDeath;
    }

    void bulletInMe(Bullet bullet){
        // Condition makes it so that you don't take damage from your own bullets or bullets you have already been shot by
        if(shotByIDs.Contains(bullet.id) || bullet.isPlayerBullet()) return; 
        shotByIDs.Add(bullet.id);
        beenShot();
        Destroy(bullet.gameObject);
        
        
    }
    void beenShot(){
        // Calling this method will trigger an event that eventually calls the damageTaken method in this class
        // Its done this way so that other classes can listen to the event and do things when the player is damaged
        Event_System.takeDamage(1, "player");
    }
    public void healPlayer(int amount){
        // notice how healing is just the player taking negative damage
        Event_System.takeDamage(-amount, "player");
    }
    void damageTaken(int damage, string to){
        if(playerHealth.isDead() || to != "player") return;
        playerHealth.takeDamage(damage);
        if(playerHealth.isDead()) Event_System.die("player");
    }

    void playerDeath(string to){
        if(to == "player")
            playerDeathAnim.play();
    }

    public void playerDeathAnimDone(){
        setPlayerUIDeathEnabled();
    }

    public void playerDeathUIShownEnough(){
        Event_System.gameOver();
    }

    void updateLifeUI(int damage, string to){
        if(to == "player"){
            health_UI_player.updateLife(playerHealth);
        }
    }

    
    void handleGetShot(){
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
         foreach (Collider2D hit in hits){
            
        	if (hit == boxCollider)
        	continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet"){
                bulletInMe(hit.gameObject.GetComponent<Bullet>());
            }
        }

    }

    void handlePlayerShoot(){
         bulletCooldownTimer += Time.deltaTime;
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey("return")) && bulletCooldownTimer >= bulletCooldown && !playerHealth.isDead()){
            bulletCooldownTimer = 0;
            gun.shoot();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp("return")){
            bulletCooldownTimer = bulletCooldown;
        }
    }

    void setPlayerUIDeathEnabled(){
        set_needed_active.setPlayerUIDeathActive();
        GameObject playerUIDeathGameObject = GameObject.Find("player_death_screen");
        if(playerUIDeathGameObject) playerUIDeath = playerUIDeathGameObject.GetComponent<Player_UI_Death>();
        playerUIDeath.beginDeathText();
    }

}
