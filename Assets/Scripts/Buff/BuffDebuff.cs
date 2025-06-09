using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffDebuff
{
    public string Name { get; set; }
    public float Duration { get; set; }
    public float Timer { get; set; }

    public BuffDebuff(string name, float duration)
    {
        Name = name;
        Duration = duration;
        Timer = duration;
    }

    public abstract void ApplyEffect(GameObject target);
    //public abstract void RemoveEffect(GameObject target);

    public void UpdateEffect(float deltaTime)
    {
        Timer -= deltaTime;
    }

    public bool IsExpired()
    {
        return Timer <= 0;
    }
}