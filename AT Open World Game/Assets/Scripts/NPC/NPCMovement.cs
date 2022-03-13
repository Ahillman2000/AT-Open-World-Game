using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    NPCSaveSystem saveSystem;

    [SerializeField] private float speed = 2f;

    Animator animationController;

    NavMeshAgent agent;

    [SerializeField] GameObject targetObject;

    void Start()
    {
        saveSystem = this.GetComponent<NPCSaveSystem>();

        animationController = this.GetComponent<Animator>();

        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(saveSystem.loaded)
        {
            if(agent != null && targetObject != null)
            {
                Vector3 targetPos = new Vector3(targetObject.transform.position.x, this.transform.position.y, targetObject.transform.position.z);

                /*if (Vector3.Distance(pos, targetPos) > agent.stoppingDistance)
                {
                    agent.destination = targetPos;
                }*/

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Trigger wave anim");
            animationController.SetTrigger("Wave");
        }
    }
}
