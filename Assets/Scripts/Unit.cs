using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int hp = 1;
    public float moveSpeed = 3f;
    public float sightRange = 5f;
    public float attackRange = 0.5f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;

    private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private UnitState currentState = UnitState.Idle;
    private Transform playerBase;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

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
        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position, sightRange);

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
            Debug.Log("Target set to " + target);
        }

        // Handle specific cases for enemy units
        if (isEnemy && (target == null || target.GetComponent<PlayerBase>() != null))
        {
            if (playerBase != null && target != playerBase)
            {
                target = playerBase;
                Debug.Log("No target, setting base to target!");
            }
        }
        // Handle the case where the target is out of sight range
        else if (target != null && Vector3.Distance(transform.position, target.position) > sightRange)
        {
            target = null;
            Debug.Log("Target out of range, setting target to null!");
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
                return true;
            }
            else if (!isEnemy && unitTarget.isEnemy) 
            { 
                return true; 
            }
            else
            {
                return false;
            }
        }
        else
        {
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
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
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
            target.GetComponent<PlayerBase>().TakeDamage(attackDamage);
        }
        else if (target.GetComponent<Unit>() != null)
        {
            target.GetComponent<Unit>().TakeDamage(attackDamage);
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
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SetState(UnitState.Dead);
        // Additional logic for death state
        Debug.Log("Unit died!");
        Destroy(gameObject);
    }

    private void SetState(UnitState newState)
    {
        currentState = newState;
    }
}
