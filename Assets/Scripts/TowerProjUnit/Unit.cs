using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Searching,
    Moving,
    Attacking,
    Dead
}

public class Unit : MonoBehaviour
{
    public bool isEnemy = true;
    public int maxhp = 1;
    public int currenthp = 0;

    public float baseMoveSpeed = 3f;
    public float modifiedMoveSpeed = 3f;

    public float baseSightRange = 5f;
    public float modifiedSightRange = 5f;

    public float baseAttackRange = 0.5f;
    public float modifiedAttackRange = 0.5f;

    public float attackSpeed = 1f;

    public int baseDamage = 1;
    public int modifiedDamage = 1;

    public int baseGoldReward = 0;
    public int modifiedGoldReward = 0;

    public int baseXPReward = 0;
    public int modifiedXPReward = 0;

    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private UnitState currentState = UnitState.Idle;
    private Transform playerBase;
    private GameManager gameManager;
    private Healthbar healthbar;

    private void Start()
    {
        gameManager = GameManager.instance;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = modifiedMoveSpeed;

        currenthp = maxhp;
        healthbar = GetComponentInChildren<Healthbar>();

        if (healthbar == null)
        {
            Debug.LogError("HealthBar component not found in children.");
        }
    }

    private void Update()
    {
        if (playerBase == null)
        {
            playerBase = FindAnyObjectByType<PlayerBase>().transform;
        }

        Targeting();
        Movement();

        // Additional logic based on the current state if needed
        switch (currentState)
        {
            case UnitState.Attacking:
                // Additional logic for attacking state
                break;
            case UnitState.Dead:
                // Additional logic for dead state
                break;
                // Handle other states if necessary
        }
    }

    private void Targeting()
    {
        if (currentState == UnitState.Dead)
        {
            // No targeting when dead
            return;
        }

        // Check for all valid unit targets in range
        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position, modifiedSightRange);

        // Set the closest valid unit as the target
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D potentialTarget in targets)
        {
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

        if (closestTarget != null)
        {
            target = closestTarget;
        }

        // Handle specific cases for enemy units
        if (isEnemy && (target == null || target.GetComponent<PlayerBase>() != null))
        {
            if (playerBase != null && target != playerBase)
            {
                target = playerBase;
            }
        }
        // Handle the case where the target is out of sight range
        else if (target != null && Vector3.Distance(transform.position, target.position) > modifiedSightRange)
        {
            target = null;
        }
    }

    private bool IsValidTarget(Collider2D potentialTarget)
    {
        // Implement your logic to determine if the potentialTarget is a valid target
        // For example, check if the potentialTarget has the Unit script attached
        Unit unitTarget = potentialTarget.GetComponent<Unit>();
        if (unitTarget)
        {
            if (isEnemy && !unitTarget.isEnemy)
            {
                unitTarget = null;
                return true;
            }
            else if (!isEnemy && unitTarget.isEnemy) 
            {
                unitTarget = null;
                return true; 
            }
            else
            {
                unitTarget = null;
                return false;
            }
        }

        Tower towerTarget = potentialTarget.GetComponent<Tower>();
        if (towerTarget)
        {
            towerTarget = null;
            return true;
        }
        else
        {
            towerTarget = null;
            return false;
        }
    }

    private void Movement()
    {
        if (currentState == UnitState.Dead)
        {
            // No movement when dead
            return;
        }

        // Set destination to the current target
        if (target != null)
        {
            if (currentState != UnitState.Attacking)
            {
                agent.SetDestination(target.position);
            } 
            else
            {
                agent.SetDestination(transform.position);
            }

            // Check if the target is within attack range
            if (Vector3.Distance(transform.position, target.position) <= modifiedAttackRange)
            {
                if (currentState != UnitState.Attacking)
                {
                    SetState(UnitState.Attacking);
                    Attack();
                }
            }
            else
            {
                SetState(UnitState.Moving);
            }
        }
        else
        {
            // No target
            SetState(UnitState.Searching);
        }
    }

    private void Attack()
    {
        if (currentState == UnitState.Dead)
        {
            // No attacking when dead
            return;
        }

        // Attack the target
        if (target.GetComponent<PlayerBase>() != null)
        {
            target.GetComponent<PlayerBase>().TakeDamage(modifiedDamage);
        }
        else if (target.GetComponent<Unit>() != null)
        {
            target.GetComponent<Unit>().TakeDamage(modifiedDamage);
        }
        else if (target.GetComponent<Tower>() != null)
        {
            target.GetComponent<Tower>().TakeDamage(modifiedDamage);
        }

        // Set a cooldown for the next attack based on attack speed
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f / attackSpeed);
        SetState(UnitState.Moving); // Switch back to moving after the cooldown
    }

    public void TakeDamage(int damage)
    {
        currenthp -= damage;
        healthbar.SetHealth((float)currenthp / maxhp);

        if (currenthp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SetState(UnitState.Dead);
        gameManager.ChangeGold(modifiedGoldReward);
        gameManager.ChangeXP(modifiedXPReward);
        Destroy(gameObject);
    }

    private void SetState(UnitState newState)
    {
        currentState = newState;
    }
}
