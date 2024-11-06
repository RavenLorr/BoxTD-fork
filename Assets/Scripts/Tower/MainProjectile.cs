using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainProjectile : MonoBehaviour
{
    public int damage = 1;
    public int penetration = 0;
    public float projectilesSpeed = 2f;
    public Vector3 targetPosition;
    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, (targetPosition - startPosition) * 100, projectilesSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject != null && col.gameObject.tag == "Enemy")
        {
            var enemy = col.gameObject.GetComponent<MainEnemyBehaviour>();
            penetration--;
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (penetration < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
