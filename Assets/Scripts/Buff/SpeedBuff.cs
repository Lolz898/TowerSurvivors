using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BuffDebuff
{
    private float speedIncrease;

    public SpeedBuff(float duration, float speedIncrease) : base("Speed Buff", duration)
    {
        this.speedIncrease = speedIncrease;
    }

    public override void ApplyEffect(GameObject target)
    {
        var buffManager = target.GetComponent<BuffDebuffManager>();
        if (buffManager != null)
        {
            buffManager.AddModifier("moveSpeed", speedIncrease, Duration);
        }
    }
    // This doesn't work properly, will result in permanant loss of stats
    /*public override void RemoveEffect(GameObject target)
    {
        var buffManager = target.GetComponent<BuffDebuffManager>();
        if (buffManager != null)
        {
            buffManager.AddModifier("moveSpeed", -speedIncrease, 0); // Instantly remove the debuff effect
        }
    }*/ 
}
