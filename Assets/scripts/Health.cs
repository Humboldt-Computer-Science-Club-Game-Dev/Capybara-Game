using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is a generic health management system that is meant to be reused when necessary in order to prevent duplicate code  */
public class Health : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public int minHealth = 0;

   
    public void takeDamage(int damage)
    {
        if(isDead()) return;
        health -= damage;
        if (health < minHealth)
        {
            health = minHealth;
        }
    }
    public void heal(int heal)
    {
        if(isDead()) return;
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }
    public bool isDead()
    {
        if (health == minHealth) return true;
        else return false;
    }
}
