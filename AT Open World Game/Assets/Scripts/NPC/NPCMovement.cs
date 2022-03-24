using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    NPCSaveSystem saveSystem;

    Animator animationController;

    private enum State
    {
        IDLE,
        JOB,
    }
    [SerializeField] State state = State.IDLE;

    NavMeshAgent agent;
    NavMeshPath navMeshPath;

    [SerializeField] Vector3 target;
    Vector3 startPos;
    [SerializeField] GameObject job;

    void Start()
    {
        saveSystem = this.GetComponent<NPCSaveSystem>();

        animationController = this.GetComponent<Animator>();

        agent = this.GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();

        startPos = this.transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                target = startPos;
                break;
            case State.JOB:
                target = job.transform.position;
                break;
        }

        Move();

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Trigger wave anim");
            animationController.SetTrigger("Wave");
        }*/
    }

    void Move()
    {
        if (saveSystem.loaded && agent != null)
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
}
