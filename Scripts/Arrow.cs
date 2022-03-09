using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;
    [HideInInspector] public float ArrowDamage;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * ArrowVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Attacked");
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            enemy.TakeDamage(ArrowDamage);

        }
        Destroy(gameObject);
    }
}
