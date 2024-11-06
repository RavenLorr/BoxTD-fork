using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainEnemyBehaviour : MonoBehaviour
{
    public int health = 1;
    public GameObject[] waypoints;
    public GameObject[] nextLayer;
    public UnityEvent<GameObject> EnemyDied = new UnityEvent<GameObject>();



    public void SetWaypoints(GameObject[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public GameObject FetchWaypointAtIndex(int index)
    {
        if (index < waypoints.Length)
        {
            return waypoints[index];
        } else
        {
            return null;
        }
    }

    public void TakeDamage(int amountOfDamage)
    {
        if(health - amountOfDamage > 0) 
        {
            health -= amountOfDamage;
        }
        else
        {
            EnemyDied.Invoke(gameObject);
            var movementBehaviour = GetComponent<EnemyMovement>();
            GameController.instance.
                StartNextLayerCoroutine(nextLayer, transform.position, movementBehaviour.targetWaypointIndex, movementBehaviour.targetWaypoint);
            Destroy(gameObject);
        }
    }
}
