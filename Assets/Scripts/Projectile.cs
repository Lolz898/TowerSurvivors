using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 1f;
    public int damage = 1;
    public float size = 1f;

    private Rigidbody2D rb;

    private void Start()
    {
        // Set the size of the projectile
        transform.localScale = new Vector3(size, size, 1);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.right * speed;

        // Set the lifetime of the projectile
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Set the velocity to move the projectile in its forward direction
        // rb.velocity = transform.up * speed;
    }

    // Function to set the damage of the projectile
    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision hit");

        // Check if the collision is with a valid enemy unit
        if (IsValidTarget(collision.collider))
        {
            Debug.Log("valid hit");

            // Deal damage to the enemy unit and destroy the projectile
            collision.collider.GetComponent<Unit>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private bool IsValidTarget(Collider2D potentialTarget)
    {
        // Check if the target is a valid enemy unit
        Unit unitTarget = potentialTarget.GetComponent<Unit>();

        return unitTarget != null && unitTarget.isEnemy;
    }
}
