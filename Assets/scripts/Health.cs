using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is a generic health management system that is meant to be reused when necessary in order to prevent duplicate code  */
public class Health : MonoBehaviour
{
    public float health = 3;
    public float maxHealth = 3;
    public float minHealth = 0;

   
    public void takeDamage(float damage)
    {
        if(isDead()) return;
        health -= damage;
        if (health < minHealth) health = minHealth;
        else if(health > maxHealth) health = maxHealth;
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
