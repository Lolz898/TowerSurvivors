using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{
    private Tower tower; // Reference to the Tower component
    private bool canAttack = true;
    private Vector3 targetLocation;

    private void Start()
    {
        tower = GetComponent<Tower>();

        if (tower == null)
        {
            Debug.LogError("Tower component not found on the same GameObject as TowerTargeting.");
        }
    }
    public void PerformAttack(Transform target)
    {
        if (canAttack)
        {
            StartCoroutine(ShootProjectiles(target));
        }
    }

    private IEnumerator ShootProjectiles(Transform target)
    {
        StartCoroutine(AttackCooldown());

        targetLocation = target.position;

        for (int i = 0; i < tower.towerData.projectileCount; i++)
        {
            ShootProjectile(targetLocation);
            yield return new WaitForSeconds(tower.towerData.multiProjDelay); // Optional delay between each projectile
        }
    }

    private void ShootProjectile(Vector3 target)
    {
        GameObject newProjectile = Instantiate(tower.towerData.projectile, transform.position, transform.rotation);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.SetDamage(tower.towerData.damage);
            projectileScript.SetLifetime(tower.towerData.projectileLifetime);
            projectileScript.SetSpeed(tower.towerData.projectileSpeed);
            projectileScript.SetSize(tower.towerData.projectileSize);
        }

        float angleDeviation = 0f;

        // Apply random deviation only if there are multiple projectiles
        if (tower.towerData.projectileCount > 1)
        {
            angleDeviation = Random.Range(-10f, 10f); // 20 degrees deviation in total
        }

        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        rotationToTarget *= Quaternion.Euler(0, 0, angleDeviation);
        newProjectile.transform.rotation = rotationToTarget;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f / tower.towerData.fireRate);
        canAttack = true;
    }
}
