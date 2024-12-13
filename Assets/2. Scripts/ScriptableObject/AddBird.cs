using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddBird", menuName = "Scriptable Object/AddBird")]
public class AddBird : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<AddOnManager>().AddBird();
        canBeInSelect = false;
        canBeInShop = false;
    }
}
