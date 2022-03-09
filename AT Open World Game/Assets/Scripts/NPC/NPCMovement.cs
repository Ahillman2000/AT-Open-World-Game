using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    NPCSaveSystem saveSystem;

    [SerializeField] private float speed = 2f;

    void Start()
    {
        saveSystem = this.GetComponent<NPCSaveSystem>();
    }

    void Update()
    {
        if(saveSystem.loaded)
        {
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
