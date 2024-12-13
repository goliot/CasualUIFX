using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenShop", menuName = "Scriptable Object/OpenShop")]
public class OpenShop : GateEffects
{
    public override void ApplyEffect(GameObject target)
    {
        GameManager.instance.uiManager.OpenShop();
    }
}
