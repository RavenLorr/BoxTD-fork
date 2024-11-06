using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : MonoBehaviour
{
    public MainTower maintower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            maintower.NewEnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            maintower.EnemyOutOfRange(other.gameObject);
        }
    }
}