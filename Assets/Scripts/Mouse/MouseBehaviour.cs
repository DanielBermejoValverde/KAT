using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        worldPosition=Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0.0f;
        transform.position = worldPosition;
    }
}
