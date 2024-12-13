using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUp", menuName = "Scriptable Object/AttackSpeedUp")]
public class AttackSpeedUp : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<PlayerController>().attackSpeed *= 0.9f;
    }
}
