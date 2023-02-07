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
    public Health playerHealth;

    private void Awake()
    {      
        playerHealth = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool isPushed = false;
    public Vector2 pushTo;
     public Vector2 pushFrom;

    public float pushTime = 0.25f;
    public float pushTimer = 0;

    private void Update()
    {
        if(!playerHealth.isDead()){
            runPlayerMovement();
        }
        else if(playerHealth.isDead() && isPushed){
            runPlayerMovement();
        }
    }

    private void runPlayerMovement(){
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
        	if (hit == boxCollider)
        	continue;

        	ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

        	if (colliderDistance.isOverlapped && hit.gameObject.tag == "Bound")
        	{
        		transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
        	}
            else if(colliderDistance.isOverlapped && hit.gameObject.tag == "Enemy" && !isPushed){
                //transform.Lerp(colliderDistance.pointA - colliderDistance.pointB);
                pushTo = 4 * (colliderDistance.pointA - colliderDistance.pointB).normalized;
                pushFrom = transform.position;
            }
        }
        if(isPushed){
            pushTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(pushFrom, pushTo, Mathf.Clamp((pushTimer / pushTime), 0, 1));
            Debug.Log("Pushing % " + Mathf.Clamp((pushTimer / pushTime), 0, 1));
            Debug.Log("Pushing Lerp " + Vector2.Lerp(transform.position, pushTo, Mathf.Clamp((pushTimer / pushTime), 0, 1)));
            float xDistance = Mathf.Abs(transform.position.x - pushTo.x);
            float yDistance = Mathf.Abs(transform.position.y - pushTo.y);
            if(xDistance < 0.01 && yDistance < 0.01){
                isPushed = false;
                pushTimer = 0;
            }
            if(pushTimer > pushTime){
                isPushed = false;
                pushTimer = 0;
            }
        }
    }

    public void onDeath(){
        Debug.Log("Player died!");
    }
}
