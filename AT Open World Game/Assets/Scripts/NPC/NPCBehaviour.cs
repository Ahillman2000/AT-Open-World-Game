using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehaviour : MonoBehaviour
{
    MeshGenerator meshGenerator;
    NPCSaveSystem saveSystem;

    private enum State
    {
        IDLE,
        JOB,
        MOVE,
    }
    [SerializeField] State state = State.IDLE;

    Vector3 idlePosition;
    GameObject job;

    NavMeshAgent agent;
    NavMeshPath navMeshPath;

    public Vector3 target;

    public int maxHealth = 100;
    public int health;

    Animator animationController;

    void Start()
    {
        meshGenerator = meshGenerator = GameObject.FindGameObjectWithTag("MeshGenerator").GetComponent<MeshGenerator>();
        saveSystem = this.GetComponent<NPCSaveSystem>();

        agent = this.GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();

        idlePosition = this.transform.position;
    }

    void Update()
    {
        if (state == State.IDLE)
        {
            target = idlePosition;
        }
        else if(state == State.JOB)
        {
            target = job.transform.position;
        }

        Move();
    }

    void Move()
    {
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


