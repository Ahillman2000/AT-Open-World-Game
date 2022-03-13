using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " has died");
    }

    void Update()
    {
        
    }
}
