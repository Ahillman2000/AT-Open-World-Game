using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    MeshGenerator meshGenerator;
    NPCSaveSystem saveSystem;

    public int maxHealth = 100;
    public int health;

    Animator animationController;

    private enum State
    {
        IDLE,
        PATROL,
        CHASE,
        FLEE,
    }
    [SerializeField] State state = State.IDLE;

    NavMeshAgent agent;
    NavMeshPath navMeshPath;

    [SerializeField] Vector3 target;

    bool patrolPointGenerated;
    Vector3 patrolPoint;
    bool fleePointGenerated;
    Vector3 fleePoint;

    void Start()
    {
        meshGenerator = GameObject.FindGameObjectWithTag("MeshGenerator").GetComponent<MeshGenerator>();
        saveSystem = this.GetComponent<NPCSaveSystem>();

        health = maxHealth;

        animationController = this.GetComponent<Animator>();

        agent = this.GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
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
            if(component.GetType() == typeof(Transform) || component.GetType() == typeof(Enemy))
            {
                continue;
            }
            else
            {
                Destroy(component);
            }
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 1f);
    }

    void Update()
    {
        switch(state)
        {
            case State.IDLE:
                target = this.transform.position;
                break;
            case State.PATROL:
                Patrol();
                break;

            case State.CHASE:
                target = Player.Instance.gameObject.transform.position;
                break;
            case State.FLEE:
                Flee();
                break;
        }

        Move();
    }
    void Patrol()
    {
        if (!patrolPointGenerated)
        {
            Vector3 startingChunkPos = saveSystem.GetStartingChunk().transform.position + new Vector3(meshGenerator.chunkSize / 2, 0, meshGenerator.chunkSize / 2);

            Vector3 offset = new Vector3(Random.Range(-meshGenerator.chunkSize / 2, meshGenerator.chunkSize / 2 + 1), this.transform.position.y, Random.Range(-meshGenerator.chunkSize / 2, meshGenerator.chunkSize / 2 + 1));

            patrolPoint = startingChunkPos + offset;
            Debug.Log("new patrol point generated: " + patrolPoint);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(patrolPoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                patrolPoint = hit.position;
                Debug.Log("patrol point adjusted to: " + patrolPoint);
            }

            if (agent.CalculatePath(patrolPoint, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                //move to target
                Debug.Log("path is valid");
                patrolPointGenerated = true;
            }
            else
            {
                Debug.Log("path is invalid");
            }
        }
        else
        {
            if (Vector3.Distance(agent.transform.position, patrolPoint) > agent.stoppingDistance)
            {
                target = patrolPoint;
                //Debug.Log("enemy moving towards patrol point: " + patrolpoint);
            }
            else
            {
                //Debug.Log("enemy reached patrol point");
                patrolPointGenerated = false;
            }
        }
    }
    void Flee()
    {
        Debug.Log("flee");
        Vector3 dirToPlayer = (this.transform.position - Player.Instance.gameObject.transform.position);
        fleePoint = this.transform.position + dirToPlayer;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            Debug.Log("flee point adjusted to: " + patrolPoint);
        }

        target = fleePoint;
    }

    void Move()
    {
        //Debug.Log(saveSystem.loaded + " " + agent + " " + target);

        if (saveSystem.loaded && agent != null && health > 0)
        {
            Vector3 targetPos = new Vector3(target.x, this.transform.position.y, target.z);

            agent.SetDestination(targetPos);

            if (agent.velocity.magnitude != 0)
            {
                animationController.SetBool("Move", true);
            }
            else
            {
                animationController.SetBool("Move", false);
            }
        }
        else
        {
            
        }
    }
}
