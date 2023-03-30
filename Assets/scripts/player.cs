using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Health_UI_Player health_UI_player;
    private Health playerHealth;
    private CharacterController2D playerController;
    private Player_Death_Anim playerDeathAnim;
    private Player_UI_Death playerUIDeath;
    private Gun gun;

    public float bulletCooldown = 0.125f;
    private float bulletCooldownTimer = 0;

    private List<int> shotByIDs;
    private BoxCollider2D boxCollider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        shotByIDs = new List<int>();
        initializeHealth();
        initializeUI();
        playerDeathAnim = GetComponent<Player_Death_Anim>();
        GameObject playerUIDeathGameObject = GameObject.Find("player_death_screen");
        playerUIDeath = playerUIDeathGameObject.GetComponent<Player_UI_Death>();
        playerUIDeathGameObject.SetActive(false);
        gun = GetComponent<Gun>();
        gun.setAsPlayer();
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
        if(!shotByIDs.Contains(bullet.id) && !bullet.isPlayerBullet()){
            shotByIDs.Add(bullet.id);
            beenShot();
            Destroy(bullet.gameObject);
        }
        
    }
    void beenShot(){
        Event_System.takeDamage(1, "player");
    }
    public void healPlayer(int amount){
        playerHealth.heal(amount);
        updateLifeUI(-amount, "player");
    }
    void damageTaken(int damage, string to){
        if(playerHealth.isDead()) return;
        if(to == "player"){
            playerHealth.takeDamage(damage);
            if(playerHealth.isDead()){
                Event_System.die("player");
            }
        }
    }

    void playerDeath(string to){
        if(to == "player"){
            playerController.onDeath();
            playerDeathAnim.play();
        }
    }

    public void playerDeathAnimDone(){
        // Gets called when expected
        playerUIDeath.beginDeathText();
    }

    public void playerDeathUIShownEnough(){
        Event_System.gameOver();
    }

    void updateLifeUI(int damage, string to){
        if(to == "player"){
            health_UI_player.updateLife(playerHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handlePlayerShoot();
        handleGetShot();
    }
    void handleGetShot(){
        // if(colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet") bulletInMe(hit.gameObject.GetComponent<Bullet>());
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

}
