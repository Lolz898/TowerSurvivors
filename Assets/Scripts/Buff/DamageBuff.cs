using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : BuffDebuff
{
    private float damageIncrease;

    public DamageBuff(float duration, float damageIncrease) : base("Damage Buff", duration)
    {
        this.damageIncrease = damageIncrease;
    }

    public override void ApplyEffect(GameObject target)
    {
        var buffManager = target.GetComponent<BuffDebuffManager>();
        if (buffManager != null)
        {
            buffManager.AddModifier("damage", damageIncrease, Duration);
        }
    }
    // This doesnt work properly, will result in permanent loss of stats
    /*public override void RemoveEffect(GameObject target)
    {
        var buffManager = target.GetComponent<BuffDebuffManager>();
        if (buffManager != null)
        {
            buffManager.AddModifier("damage", -damageIncrease, 0); // Instantly remove the buff effect
        }
    }*/
}
