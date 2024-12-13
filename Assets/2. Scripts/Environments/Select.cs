using TMPro;
using UnityEngine;

public class Select : MonoBehaviour
{
    public GateEffects effect; // �ϳ��� Effect�� �Ҵ�޾� ����
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
