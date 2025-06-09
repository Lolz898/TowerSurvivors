using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour, IAttack
{
    private Tower tower; // Reference to the Tower component
    private bool canAttack = true;
    public GameObject areaEffectPrefab; // Reference to the circle effect prefab
    public float effectDuration = 0.5f; // Duration to display the effect

    private void Start()
    {
        tower = GetComponent<Tower>();

        if (tower == null)
        {
            Debug.LogError("Tower component not found on the same GameObject as AreaAttack.");
        }
    }

    public void PerformAttack(Transform target)
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown());

            // Instantiate the area effect at the tower's position
            GameObject areaEffect = Instantiate(areaEffectPrefab, transform.position, Quaternion.identity);
            areaEffect.transform.localScale = new Vector3(tower.towerData.range * 2, tower.towerData.range * 2, 1);
            Destroy(areaEffect, effectDuration); // Destroy the effect after the specified duration

            Collider2D[] unitsInRange = Physics2D.OverlapCircleAll((Vector2)transform.position, tower.towerData.range);

            foreach (Collider2D unit in unitsInRange)
            {
                if (tower.IsValidTarget(unit))
                {
                    unit.GetComponent<Unit>().TakeDamage(tower.towerData.damage);
                }
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f / tower.towerData.fireRate);
        canAttack = true;
    }
}
