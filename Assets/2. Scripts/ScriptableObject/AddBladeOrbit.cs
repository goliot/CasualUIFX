using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddBladeOrbit", menuName = "Scriptable Object/AddBladeOrbit")]
public class AddBladeOrbit : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<AddOnManager>().AddOuterBlade();
    }
}
