using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float speed;
     private Vector3 offset; // Offset between camera and player

    void Start()
    {
        // Calculate and store the initial offset between the camera and the player
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
