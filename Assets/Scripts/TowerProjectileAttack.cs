using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{
    private Tower tower; // Reference to the Tower component
    private bool canAttack = true;

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
            GameObject newProjectile = Instantiate(tower.towerData.projectile, transform.position, transform.rotation);
            Projectile projectileScript = newProjectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.SetDamage(tower.towerData.damage);
            }

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Quaternion rotationToTarget = Quaternion.LookRotation(Vector3.forward, directionToTarget);
            newProjectile.transform.rotation = rotationToTarget;

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f / tower.towerData.fireRate);
        canAttack = true;
    }
}
