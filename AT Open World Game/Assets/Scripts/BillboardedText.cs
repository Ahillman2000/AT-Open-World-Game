using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardedText : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;

    void Start()
    {
    }

    void Update()
    {
        this.transform.LookAt(mainCamera.transform.position);
    }
}
