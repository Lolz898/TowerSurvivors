using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour, IAttack
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
            Collider2D[] unitsInRange = Physics2D.OverlapCircleAll((Vector2)transform.position, tower.towerData.range);

            foreach (Collider2D unit in unitsInRange)
            {
                if (tower.IsValidTarget(unit))
                {
                    unit.GetComponent<Unit>().TakeDamage(tower.towerData.damage);
                }
            }

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
