using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effects
{
    AddBird,
    AddBlade,
    AddBladeOrbit,
    OpenShop,
    AddShuriken,
    AttackUp,
    AttackSpeedUp,
    Heal15,
}

public abstract class GateEffects : ScriptableObject
{
    public string desc; // Ό³Έν
    public Effects effectType;
    public bool canBeInShop = true;
    public bool canBeInSelect = true;
    public int price;

    public abstract void ApplyEffect(GameObject target);
}
