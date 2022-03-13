using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;

    Animator animationController;

    void Start()
    {
        health = maxHealth;

        animationController = this.GetComponent<Animator>();
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
        //Debug.Log(gameObject.name + " has died");
        animationController.SetTrigger("Die");
        Unload();
    }

    public void Unload()
    {
        Destroy(this.GetComponent<NavMeshAgent>());
        Destroy(this.GetComponent<CapsuleCollider>());
    }

    public void DestroyThis()
    {
        //Destroy(this.gameObject);

        foreach (Component component in this.GetComponents(typeof(Component)))
        {
            if(component.GetType() != typeof(Transform))
            {
                Destroy(component);
            }
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }
}
