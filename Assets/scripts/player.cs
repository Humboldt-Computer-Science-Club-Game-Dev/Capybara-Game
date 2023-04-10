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
        checkIfPlayerIsShot();
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

    void checkIfPlayerIsShot(){
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
         foreach (Collider2D hit in hits){
            
        	if (hit == boxCollider)  // Ignore collision with self
        	continue;

            // Get the distance between the two colliders
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet")
                bulletInMe(hit.gameObject.GetComponent<Bullet>());
        }
    }

    void bulletInMe(Bullet bullet){
        // Condition makes it so that you don't take damage from your own bullets or bullets you have already been shot by
        if(shotByIDs.Contains(bullet.id) || bullet.isPlayerBullet()) return; 
        takeShotDamage();
        shotByIDs.Add(bullet.id);
        Destroy(bullet.gameObject);
        
        
    }
    void takeShotDamage(){
        // Calling this method will trigger an event that eventually calls the damageTaken method in this class
        // Its done this way so that other classes can listen to the event and do things when the player is damaged
        Event_System.takeDamage(1, "player");
    }

    public void healPlayer(int amount){
        // notice how healing is just the player taking negative damage
        Event_System.takeDamage(-amount, "player");
    }

    //Calling takeShotDamage will trigger an event that will then call this method
    void damageTaken(int damage, string to){
        if(playerHealth.isDead() || to != "player") return;
        playerHealth.takeDamage(damage);
        if(playerHealth.isDead()) Event_System.die("player");
    }

    //This method is called by the event system when the anything takes damage
    //This is why you need to check if the damage is for the player
    void updateLifeUI(int damage, string to){
        if(to == "player")
            health_UI_player.updateLife(playerHealth);
    }

    //This method is called by the event system when the anything takes damage
    //This is why you need to check if the damage is for the player
    void playerDeath(string to){
        if(to == "player")
            playerDeathAnim.play();
    }

    // Gets called by the Player_Death_Anim script when the death animation is done
    public void playerDeathAnimDone(){
        // Sets player_death_screen to active so it can be found in the next line of code
        set_needed_active.setPlayerUIDeathActive();

        GameObject playerUIDeathGameObject = GameObject.Find("player_death_screen");
        if(playerUIDeathGameObject) playerUIDeath = playerUIDeathGameObject.GetComponent<Player_UI_Death>();
        if (playerUIDeath) playerUIDeath.beginDeathText();
    }

    // Gets called by the Player_UI_Death script when the death text has been displayed for a predetermined amount of time
    public void playerDeathUIShownEnough(){
        Event_System.gameOver();
    }

    void handlePlayerShoot(){
         bulletCooldownTimer += Time.deltaTime;
        
        // Player must press either of the shoot buttons, the cooldown must be over, and the player must not be dead
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey("return")) && bulletCooldownTimer >= bulletCooldown && !playerHealth.isDead()){
            bulletCooldownTimer = 0;
            gun.shoot();
        }

        // Makes it so that the player can shoot as fast as they want so long as they manually click the shoot button
        if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp("return"))
            bulletCooldownTimer = bulletCooldown;
        
    }
}
