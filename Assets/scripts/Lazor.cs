using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazor : MonoBehaviour
{
    public float lazorCooldown = 4f;
    public float lazorCooldownTimer = 0;
    public float coolDownSpeed = 1f;
    public float heatUpSpeed = 1f;
    bool overHeatLock = false;

    SpriteRenderer lazorSpriteRenderer;
    GameObject lazorGraphicObject;
    BoxCollider2D lazorBoxCollider2D;
    bool lazoring = true;

    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        if (started) return;
        getComponents();
        started = true;
    }

    void Update()
    {
        if (overHeatLock) coolDown();

        if (lazorCooldownTimer <= 0)
        {
            overHeatLock = false;
            lazorCooldownTimer = 0;
        }

        handleLazoring();
    }

    void handleLazoring()
    {
        if (!lazorBoxCollider2D.enabled) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, lazorBoxCollider2D.size, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit == lazorBoxCollider2D)  // Ignore collision with self
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(lazorBoxCollider2D);

            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet")
            {
                Bullet hitBullet = hit.gameObject.GetComponent<Bullet>();
                if (hitBullet.side != Gun.sideOptions.player)
                {
                    Destroy(hit.gameObject);
                }
            }

            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Enemy")
            {
                Enemy hitEnemy = hit.gameObject.GetComponent<Enemy>();
                int id = hitEnemy.getID();
                Event_System.takeDamage(10 * Time.deltaTime, "enemy" + id);
            }
        }
    }

    void getComponents()
    {
        lazorGraphicObject = GameObject.Find("lazor_graphic");
        lazorBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void lazor()
    {
        bool isOverHeated = lazorCooldownTimer >= lazorCooldown;

        if (!isOverHeated && !overHeatLock)
        {
            activateLazor();
            lazorCooldownTimer += Time.deltaTime * heatUpSpeed;
        }
        else if (isOverHeated)
        {
            stopLazor();
            overHeatLock = true;
        }

    }
    public void stopLazor()
    {
        if (!started) Start();
        coolDown();
        if (!lazoring) return;
        lazoring = false;
        lazorGraphicObject.SetActive(false);
        lazorBoxCollider2D.enabled = false;
    }

    public void coolDown()
    {
        lazorCooldownTimer -= Time.deltaTime * coolDownSpeed;
        if (lazorCooldownTimer < 0) lazorCooldownTimer = 0;
    }

    void activateLazor()
    {
        if (lazoring) return;
        lazoring = true;
        lazorGraphicObject.SetActive(true);
        lazorBoxCollider2D.enabled = true;
    }
}
