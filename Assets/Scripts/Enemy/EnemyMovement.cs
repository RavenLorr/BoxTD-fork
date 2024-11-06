using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public MainEnemyBehaviour behaviour;
    public static float baseSpeed = 1f;
    public float speedMultiplier = 1f;
    public int targetWaypointIndex = 0;
    public GameObject targetWaypoint;


    // Update is called once per frame
    void Update()
    {
        tryMovement();
    }

    private void tryMovement()
    {
        if (targetWaypointIndex != 0 && targetWaypoint == null) return;
        if (targetWaypointIndex == 0) 
        {
            targetWaypoint = behaviour.FetchWaypointAtIndex(targetWaypointIndex);
        } 

        Vector3 targetPos = targetWaypoint.transform.position;


        float step = baseSpeed * speedMultiplier * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            targetWaypoint = behaviour.FetchWaypointAtIndex(++targetWaypointIndex);
        }
    }
}
