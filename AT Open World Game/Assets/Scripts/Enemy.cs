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

    [SerializeField] EnemyState state = EnemyState.IDLE;

    NavMeshAgent agent;
    NavMeshPath navMeshPath;

    public Vector3 target;

    bool patrolPointGenerated;
    Vector3 patrolpoint;

    [SerializeField] GameObject patrolPointDebugObject;

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 1f);
    }

    void Update()
    {
        if(state == EnemyState.IDLE)
        {
            target = this.transform.position;
        }
        else if(state == EnemyState.PATROL)
        {
            Patrol();
        }
        else if (state == EnemyState.CHASE)
        {
            Flee();
        }
        else if (state == EnemyState.FLEE)
        {
            Vector3 dirToPlayer = (this.transform.position - Player.Instance.gameObject.transform.position).normalized;
            Vector3 fleePos = this.transform.position + dirToPlayer;
            target = fleePos;
        }

        Move();
    }
    void Patrol()
    {
        if (!patrolPointGenerated)
        {
            Vector3 startingChunkPos = saveSystem.GetStartingChunk().transform.position + new Vector3(meshGenerator.chunkSize / 2, 0, meshGenerator.chunkSize / 2);

            Vector3 offset = new Vector3(Random.Range(-meshGenerator.chunkSize / 2, meshGenerator.chunkSize / 2 + 1), this.transform.position.y, Random.Range(-meshGenerator.chunkSize / 2, meshGenerator.chunkSize / 2 + 1));

            patrolpoint = startingChunkPos + offset;
            Debug.Log("new patrol point generated: " + patrolpoint);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(patrolpoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                patrolpoint = hit.position;
                Debug.Log("patrol point adjusted to: " + patrolpoint);
            }

            if (agent.CalculatePath(patrolpoint, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                //move to target
                //Debug.Log("path is valid");
                patrolPointGenerated = true;
            }
            else
            {
                Debug.Log("path is invalid");
            }
        }
        else
        {
            if (Vector3.Distance(agent.transform.position, patrolpoint) > agent.stoppingDistance)
            {
                target = patrolpoint;
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
        Vector3 dirToPlayer = (this.transform.position - Player.Instance.gameObject.transform.position).normalized;
        Vector3 fleePos = this.transform.position + dirToPlayer;
        target = fleePos;
    }

    void Move()
    {
        //Debug.Log(saveSystem.loaded + " " + agent + " " + target);

        if (saveSystem.loaded)
        {
            if (agent != null)
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
        }
        else
        {
            
        }
    }
}

public enum EnemyState
{
    IDLE,
    PATROL,
    CHASE,
    FLEE,
}
