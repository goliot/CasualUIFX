using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackUp", menuName = "Scriptable Object/AttackUp")]
public class AttackUp : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<PlayerController>().damage *= 1.1f;
    }
}
