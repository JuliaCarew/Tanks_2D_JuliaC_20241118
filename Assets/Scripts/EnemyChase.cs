using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    

    void Update()
    {    
        // direction rotates the enemy towards the player using vector subtraction
        Vector2 direction = (player.transform.position - transform.position).normalized;  // normalizing so only the direction to rotate is accounted for

        // getting the X and Y positions of the vector2 direction * RadianToDegree
        float angleToRotate = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // === MOVE === //
        // update the enemy to move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        // === ROTATE === //
        // update the enemy to rotate and look at the player
        // in Euler brackets put in any vector we want to apply the math to
        transform.rotation = Quaternion.Euler(Vector3.forward * angleToRotate);
    }

}
/*
   // ! UNUSED ! //
    private float distance;
    // !!! UNUSED !!! //
    // variable distance gets two vectors and gets the magnitude / length
    // ??? use this when getting dot product / result of if obstacles are in the enemies' line of sight OR find another way
    distance = Vector2.Distance(transform.position, player.transform.position); 

    get enemies to stop chasing when line of sight is blocked (get dot product between enemy & player)

*/