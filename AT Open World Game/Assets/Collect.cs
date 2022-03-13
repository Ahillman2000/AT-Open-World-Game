using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private bool pickedUp;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp)
        {
            pickedUp = true;
            this.gameObject.SetActive(false);
        }
    }

    public bool PickedUp()
    {
        return pickedUp;
    }

    void Update()
    {

    }
}
