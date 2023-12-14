using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int goldCost = 10;
    public int health = 50;
    public float range = 6f;
    public int damage = 1;
    public float fireRate = 1f;

    public bool isProjectile = false;
    public bool isArea = false;

    public GameObject projectile;

    private Transform target;
    private bool canAttack = true;

    void Update()
    {
        Targeting();
        
        if (target != null)
        {
            Attack();
        }
    }

    private void Targeting()
    {
        // Check for all valid unit targets in range
        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position, range);

        // Set the closest valid unit as the target
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D potentialTarget in targets)
        {
            // Check if the potential target is valid and not the tower itself
            if (IsValidTarget(potentialTarget) && potentialTarget.transform != transform)
            {
                float distanceToTarget = Vector3.Distance(transform.position, potentialTarget.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = potentialTarget.transform;
                }
            }
        }

        // Set the closest target as the current target
        if (closestTarget != null)
        {
            target = closestTarget;
        }

        // Check if the current target is out of range
        if (target != null && Vector3.Distance(transform.position, target.position) > range)
        {
            // Reset target to null if out of range
            target = null;
            Debug.Log("Target out of range, setting target to null!");
        }
    }

    void Attack()
    {
        if (target != null)
        {
            if (isProjectile)
            {
                ProjectileAttack();
            }

            if (isArea)
            {
                AreaAttack();
            }
        }
    }

    void ProjectileAttack()
    {
        if (canAttack)
        {
            // Instantiate the projectile at the tower's position and rotation
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);

            // Get the Projectile script component from the instantiated projectile
            Projectile projectileScript = newProjectile.GetComponent<Projectile>();

            // Pass tower's damage to the projectile
            if (projectileScript != null)
            {
                projectileScript.SetDamage(damage);
            }

            // Rotate the projectile to face the target
            if (target != null)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                Quaternion rotationToTarget = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                newProjectile.transform.rotation = rotationToTarget;
            }

            StartCoroutine(AttackCooldown());
        }
    }

    void AreaAttack()
    {
        if (canAttack)
        {
            // Perform area attack in range
            Collider2D[] unitsInRange = Physics2D.OverlapCircleAll((Vector2)transform.position, range);

            foreach (Collider2D unit in unitsInRange)
            {
                if (IsValidTarget(unit))
                {
                    // Deal damage to the unit
                    unit.GetComponent<Unit>().TakeDamage(damage);
                }
            }

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f / fireRate);
        canAttack = true;
    }

    bool IsValidTarget(Collider2D potentialTarget)
    {
        // Check if the target is valid
        Unit unitTarget = potentialTarget.GetComponent<Unit>();
        
        if (unitTarget != null && unitTarget.isEnemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Implement death logic, e.g., play death animation, remove object, etc.
        Debug.Log("Tower destroyed!");
        Destroy(gameObject);
    }
}
