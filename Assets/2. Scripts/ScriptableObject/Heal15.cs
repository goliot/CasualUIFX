using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal15", menuName = "Scriptable Object/Heal15")]
public class Heal15 : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        PlayerController player = target.GetComponent<PlayerController>();
        float healAmount = player.maxHealth * 0.15f;
        float healthDifference = player.maxHealth - player.health;
        float healingToApply = Mathf.Min(healAmount, healthDifference);

        player.health += healingToApply;
    }
}
