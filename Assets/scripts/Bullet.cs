using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float playerBulletSpeed = 0.5f;
    public float enemyBulletSpeed = 0.5f;
    private CircleCollider2D circleCollider;
    [HideInInspector]
    public Gun.sideOptions side;
    [HideInInspector]
    public int id = 0;
    Animator animator;
    bool initialized = false;

    void Awake()
    {
        // It is up to the gun script that instantiates this bullet to set the side of the bullet
        side = Gun.sideOptions.undecided;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        if (initialized) return;
        id = this.gameObject.GetInstanceID();
        animator = transform.Find("animation").gameObject.GetComponent<Animator>();
        initialized = true;
    }
    void Update()
    {
        handleCollision();
    }
    void FixedUpdate()
    {
        handleMovement();
    }
    private void handleCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (Collider2D hit in hits)
        {
            if (hit == circleCollider) continue; // Ignore collision with self
            ColliderDistance2D colliderDistance = hit.Distance(circleCollider); // Get the distance between the two colliders

            // If the bullet goes off screen, destroy it
            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet_Bound")
            {
                Destroy(this.gameObject);
            }

            // If the bullet hits another bullet, destroy both
            if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bullet")
            {
                Destroy(this.gameObject);
                Destroy(hit.gameObject);
            }
        }
    }
    private void handleMovement()
    {
        if (side == Gun.sideOptions.player)
        {
            // Move the bullet to the right
            this.transform.position = new Vector3(this.transform.position.x + getSpeed(), this.transform.position.y, this.transform.position.z);
        }
        else if (side == Gun.sideOptions.enemy)
        {
            // Move the bullet to the left
            this.transform.position = new Vector3(this.transform.position.x - getSpeed(), this.transform.position.y, this.transform.position.z);
        }
    }

    public void setSide(Gun.sideOptions side)
    {
        if (!initialized) Start();
        /* `side` is a variable that is used to determine which side the bullet is on. */
        this.side = side;
        if (side == Gun.sideOptions.player)
        {
            float randomStartPoint = Random.Range(0.0f, 1.0f);
            animator.Play("pineapple", -1, randomStartPoint);

        }
        else if (side == Gun.sideOptions.enemy)
        {
            float randomStartPoint = Random.Range(0.0f, 1.0f);
            animator.Play("egg", -1, randomStartPoint);
        }
    }
    public bool isEnemyBullet()
    {
        return side == Gun.sideOptions.enemy;
    }
    public bool isPlayerBullet()
    {
        return side == Gun.sideOptions.player;
    }
    float getSpeed()
    {
        if (side == Gun.sideOptions.player) return playerBulletSpeed;
        else return enemyBulletSpeed;
    }

}
