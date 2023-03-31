using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pack : MonoBehaviour
{
    public int healAmount = 1;
    public float gravitySpeed = 0f;
    private BoxCollider2D boxCollider;
    private Transform playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        playerLocation = GameObject.Find("capy").GetComponent<Transform>();
    }

    public float lifetime = 10f;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifetime){
            Destroy(this.gameObject);
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        foreach (Collider2D hit in hits){
            
        	if (hit == boxCollider)
        	continue;

           if(hit.gameObject.tag == "Player"){
              hit.gameObject.GetComponent<player>().healPlayer(healAmount);
               Destroy(this.gameObject);
           }
        }
    }

    void FixedUpdate(){
        transform.position = Vector2.MoveTowards(transform.position, playerLocation.position, gravitySpeed * Time.deltaTime);
    }
}
