using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainTower : MonoBehaviour
{
    public string name;
    public float attackSpeed = 1.0f;
    private float delay;
    public GameObject projectilesContainer;
    public GameObject projectilePrefab; // Renamed to better reflect its purpose
    private List<GameObject> possibleTargets = new List<GameObject>(); // Initialize the list
    private float lastShotTime = 0;
    public bool isPlaced = false;


    public void NewEnemyInRange(GameObject enemy)
    {
        possibleTargets.Add(enemy);
        enemy.GetComponent<MainEnemyBehaviour>().EnemyDied.AddListener(OnEnemyDied);
    }

    public void EnemyOutOfRange(GameObject enemy)
    {
        possibleTargets.Remove(enemy);
    }

    public void Update()
    {
        if (isPlaced)
        {
            delay = (1 / attackSpeed);
            if (possibleTargets.Count > 0 && lastShotTime + delay < Time.time)
            {
                lastShotTime = Time.time;
                var enemy = possibleTargets[0];
                var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, projectilesContainer.transform);
                projectile.GetComponent<MainProjectile>().targetPosition = enemy.transform.position;
            }
        }
    }

    public void OnEnemyDied(GameObject enemy) { possibleTargets.Remove(enemy); }
}
