using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float timeAlive;
    private float lifespan;
    private EnemyStateManager enemy;
    private int damage = 10;

    void Start()
    {
        timeAlive = 0f;
        lifespan = 3f;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifespan)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            enemy = collision.collider.gameObject.GetComponent<EnemyStateManager>();
            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
