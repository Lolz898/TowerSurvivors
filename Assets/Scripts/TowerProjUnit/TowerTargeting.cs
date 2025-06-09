using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    private Transform target;
    private Tower tower;

    private void Start()
    {
        // Get reference to the Tower script on the same GameObject
        tower = GetComponent<Tower>();

        if (tower == null)
        {
            Debug.LogError("Tower component not found on the same GameObject as TowerTargeting.");
        }
    }

    public Transform GetTarget()
    {
        // Implement targeting logic here, based on tower's parameters
        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position, tower.towerData.range);
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D potentialTarget in targets)
        {
            if (tower.IsValidTarget(potentialTarget) && potentialTarget.transform != transform)
            {
                float distanceToTarget = Vector3.Distance(transform.position, potentialTarget.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = potentialTarget.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
        }
        else
        {
            target = null;
        }

        return target;
    }
}
