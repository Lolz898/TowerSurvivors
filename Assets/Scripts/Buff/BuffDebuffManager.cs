using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffManager : MonoBehaviour
{
    // Dictionaries to store additive modifiers for each attribute
    private Dictionary<string, float> attributeModifiers = new Dictionary<string, float>();

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        // Initialize attribute modifiers
        attributeModifiers["damage"] = 0f;
        attributeModifiers["moveSpeed"] = 0f;
        attributeModifiers["attackRange"] = 0f;
        attributeModifiers["sightRange"] = 0f;
        attributeModifiers["goldReward"] = 0f;
        attributeModifiers["xpReward"] = 0f;
    }

    public void AddModifier(string attribute, float value, float duration)
    {
        if (attributeModifiers.ContainsKey(attribute))
        {
            attributeModifiers[attribute] += value;
            StartCoroutine(RemoveModifierAfterDelay(attribute, value, duration));
        }
        else
        {
            Debug.LogError("Attribute not found: " + attribute);
        }
        ApplyModifiers();
    }

    private IEnumerator RemoveModifierAfterDelay(string attribute, float value, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (attributeModifiers.ContainsKey(attribute))
        {
            attributeModifiers[attribute] -= value;
        }
        ApplyModifiers();
    }

    private void ApplyModifiers()
    {
        if (unit != null)
        {
            unit.modifiedDamage = unit.baseDamage + (int)attributeModifiers["damage"];
            unit.modifiedMoveSpeed = unit.baseMoveSpeed + attributeModifiers["moveSpeed"];
            unit.modifiedAttackRange = unit.baseAttackRange + attributeModifiers["attackRange"];
            unit.modifiedSightRange = unit.baseSightRange + attributeModifiers["sightRange"];
            unit.modifiedGoldReward = unit.baseGoldReward + (int)attributeModifiers["goldReward"];
            unit.modifiedXPReward = unit.baseXPReward + (int)attributeModifiers["xpReward"];
        }
    }
}
