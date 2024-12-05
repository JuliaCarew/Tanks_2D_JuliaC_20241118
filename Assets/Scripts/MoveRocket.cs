using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRocket : MonoBehaviour
{
    // projectile variables
    private float speed;
    private float lifeTime;
    private float timer;

    // for projectile tracking
    private Transform target; // Current target
    private float rotationSpeed = 5f; // missile rotates to face target

    // for visual line trail
    private LineRenderer lineRenderer; 
    private List<Vector3> pathPoints = new List<Vector3>(); // store the trail positions

    public void Initialize(Transform targetEnemy, float bulletSpeed, float bulletLifeTime)
    {
        speed = bulletSpeed;
        lifeTime = bulletLifeTime;
        target = targetEnemy;

        lineRenderer = GetComponent<LineRenderer>(); // get prefabs linerenderer component
  
        if (lineRenderer != null)  // initialize the trail positions
        {
            pathPoints.Add(transform.position); // initial position
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, transform.position);
        }
    }

    void Update()
    {
         if (target != null)
        {
            // calculate direction towards the target
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // gradually rotate towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // move the missile forward
            transform.position += transform.up * speed * Time.deltaTime;
        }
        else
        {
            // if no target, move straight forward
            transform.position += transform.up * speed * Time.deltaTime;
        }

        // add the missile's current position to the trail
        if (lineRenderer != null)
        {
            pathPoints.Add(transform.position);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
        }

        // destroy the missile after its lifetime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
