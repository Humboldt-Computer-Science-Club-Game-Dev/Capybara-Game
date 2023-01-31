using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int minHealth = 0;

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < minHealth)
        {
            health = minHealth;
        }
        Debug.Log("Health: " + health);
    }
    public bool isDead()
    {
        if (health == minHealth) return true;
        else return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
