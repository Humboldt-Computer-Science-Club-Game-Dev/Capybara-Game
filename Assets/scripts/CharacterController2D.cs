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

    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private bool isPushed = false;
    private Vector2 pushTo;

    private float pushTime = 0.5f;
    private float pushTimer = 0;

    private void Update()
    {
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
                pushTo = 4 * (colliderDistance.pointA - colliderDistance.pointB);
                isPushed = true;
            }
        }
        if(isPushed){
            pushTimer += Time.deltaTime;
                transform.position = Vector2.Lerp(transform.position, pushTo, 0.05f);
                float xDistance = Mathf.Abs(transform.position.x - pushTo.x);
                float yDistance = Mathf.Abs(transform.position.y - pushTo.y);
                if(xDistance < 0.1 && yDistance < 0.1){
                    isPushed = false;
                }
                if(pushTimer > pushTime){
                    isPushed = false;
                    pushTimer = 0;
                }
            }

    }
}
