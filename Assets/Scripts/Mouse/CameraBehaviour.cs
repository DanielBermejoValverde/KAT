using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float distanceToPlayer =5f;
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition) + playerPosition;
        pos.z = -10;
        pos.x = Mathf.Clamp(pos.x, -distanceToPlayer + playerPosition.x, distanceToPlayer);
        pos.y = Mathf.Clamp(pos.y, -distanceToPlayer + playerPosition.y, distanceToPlayer);
        this.transform.position = Vector3.MoveTowards(transform.position,pos,speed);
    }
}
