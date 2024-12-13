using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddShuriken", menuName = "Scriptable Object/AddShuriken")]
public class AddShuriken : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<AddOnManager>().AddShuriken();
    }
}
