using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    NPCSaveSystem saveSystem;

    [SerializeField] private float speed = 2f;

    Animator animationController;

    void Start()
    {
        saveSystem = this.GetComponent<NPCSaveSystem>();

        animationController = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(saveSystem.loaded)
        {
            //this.transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Trigger wave anim");
            animationController.SetTrigger("Wave");
        }
    }
}
