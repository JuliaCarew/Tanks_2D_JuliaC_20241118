using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public Camera cam;
    Vector2 mousePos;

    void Update()
    {
        // getting axis input to update player input
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        // transforming player position * speed
        transform.position = transform.position + playerInput.normalized * speed * Time.deltaTime;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);      
        // getting angle for player aim rotation
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 0f;

        transform.rotation = Quaternion.Euler( 0, 0, lookAngle);
    }
}
