using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData towerData;
    private int currenthp;
    private Transform target;
    private Healthbar healthbar;
    private TowerTargeting targetingScript;
    private IAttack attackScript;
    private TileOccupier tileOccupier;
    private GameManager gameManager;

    private void Start()
    {
        InitializeTower();

        gameManager = GameManager.instance;
        tileOccupier = gameManager.GetComponentInChildren<TileOccupier>();
    }

    private void InitializeTower()
    {
        currenthp = towerData.maxhp;
        healthbar = GetComponentInChildren<Healthbar>();

        if (healthbar == null)
        {
            Debug.LogError("HealthBar component not found in children.");
        }

        targetingScript = GetComponent<TowerTargeting>();

        if (targetingScript == null)
        {
            Debug.LogError("TowerTargeting component not found on the tower.");
        }

        attackScript = GetComponent<IAttack>();
    }

    void Update()
    {
        if (targetingScript == null)
        {
            return;
        }

        Transform target = targetingScript.GetTarget();

        if (target != null)
        {
            Attack(target);
        }
    }

    void Attack(Transform target)
    {
        if (attackScript != null)
        {
            attackScript.PerformAttack(target); // Delegate attack to the current attack script
        }
        else
        {
            Debug.LogError("No attack script found!");
        }
    }

    public bool IsValidTarget(Collider2D potentialTarget)
    {
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
        currenthp -= damageTaken;
        healthbar.SetHealth((float)currenthp / towerData.maxhp);

        if (currenthp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Implement death logic, e.g., play death animation, remove object, etc.
        tileOccupier.DeOccupyTiles(transform.position);
        Destroy(gameObject);
    }
}
