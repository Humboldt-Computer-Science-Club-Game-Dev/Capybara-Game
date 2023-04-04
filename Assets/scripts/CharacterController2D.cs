using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while player presses movement inputs.")]
    float acceleration = 75;

    [SerializeField, Tooltip("Deceleration applied when character is not attempting to move.")]
    float deceleration = 70;

    private BoxCollider2D boxCollider;
    private Vector2 velocity;
    [HideInInspector]
    public Health playerHealth;
    [HideInInspector]
    public bool isPushed = false;
    private Vector2 pushTo;
    private Vector2 pushFrom;

    public float pushTime = 0.25f;
    private float pushTimer = 0;

    float pushDistanceX;
    float pushDistanceY;

    private void Awake()
    {      
        getComponents();
    }

    private void Update()
    {
        runPlayerMovement();
    }

    void getComponents(){
        playerHealth = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    bool canPlayerMove(){
        bool isDead = playerHealth.isDead();
        return !isDead || (isDead && isPushed);
    }
    private void runPlayerMovement(){
        if(!canPlayerMove()) return;

        transform.Translate(velocity * Time.deltaTime);
        float horizontalMoveInput = Input.GetAxisRaw("Horizontal");
        float verticalMoveInput = Input.GetAxisRaw("Vertical");
        if (horizontalMoveInput != 0) velocity.x = Mathf.MoveTowards(velocity.x, speed * horizontalMoveInput, acceleration * Time.deltaTime);
        else velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        if (verticalMoveInput != 0) velocity.y = Mathf.MoveTowards(velocity.y, speed * verticalMoveInput, acceleration * Time.deltaTime);
        else velocity.y = Mathf.MoveTowards(velocity.y, 0, deceleration * Time.deltaTime);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
        	if (hit == boxCollider) continue; // Ignore collision with self

            // Get the distance between the two colliders
        	ColliderDistance2D colliderDistance = hit.Distance(boxCollider);  

            // If the player goes off screen, move them back on screen
        	if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bound")
        		transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
        }
        handleGettingPushed();
    }

    // Gets called from pusher
    public void getPushed(Vector2 pushTo){
        isPushed = true;
        this.pushTo = pushTo;
        pushFrom = transform.position;
    }

    void handleGettingPushed(){
        if(!isPushed) return;
        pushTimer += Time.deltaTime;
        transform.position = Vector2.Lerp(pushFrom, pushTo, Mathf.Clamp((pushTimer / pushTime), 0, 1));
        if(hasFinishedPush()){
            isPushed = false;
            pushTimer = 0;
        }
    }

    bool hasFinishedPush(){
        pushDistanceX = Mathf.Abs(transform.position.x - pushTo.x);
        pushDistanceY = Mathf.Abs(transform.position.y - pushTo.y);
        return ((pushDistanceX < 0.01 && pushDistanceY < 0.01) || (pushTimer > pushTime));
    }
}
