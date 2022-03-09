using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject mainCamera;

    void Start()
    {
        
    }

    void Update()
    {
        text.transform.LookAt(mainCamera.transform.position);
    }
}
