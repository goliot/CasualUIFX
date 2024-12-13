using TMPro;
using UnityEngine;

public class Select : MonoBehaviour
{
    public GateEffects effect; // 하나의 Effect만 할당받아 적용
    public TextMeshPro desc;

    public void SetUp()
    {
        desc.text = effect.desc;
    }

    public void ActivateEffect(GameObject target)
    {
        if (effect != null)
        {
            effect.ApplyEffect(target);
        }
    }
}
