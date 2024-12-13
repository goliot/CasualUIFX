using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddBlade", menuName = "Scriptable Object/AddBlade")]
public class AddBlade : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        int idx = Random.Range(0, target.GetComponent<AddOnManager>().blades.Count);
        target.GetComponent<AddOnManager>().AddBlade(idx);
    }
}
