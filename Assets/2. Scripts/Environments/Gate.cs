using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public List<GateEffects> effects;
    public Select leftSelect;
    public Select rightSelect;

    private void OnEnable()
    {
        effects = GameManager.instance.effects;
        List<GateEffects> effectsClone = new List<GateEffects>(effects); // 클론 리스트 생성

        int ranIndexLeft = Random.Range(0, effectsClone.Count);

        while (!effectsClone[ranIndexLeft].canBeInSelect)
        {
            ranIndexLeft = (ranIndexLeft + 1) % effectsClone.Count;
        }

        leftSelect.effect = effects[ranIndexLeft];
        leftSelect.SetUp();
        effectsClone.RemoveAt(ranIndexLeft);

        int ranIndexRight = Random.Range(0, effectsClone.Count);
        while (!effectsClone[ranIndexRight].canBeInSelect)
        {
            ranIndexRight = (ranIndexRight + 1) % effectsClone.Count;
        }
        rightSelect.effect = effects[ranIndexRight];
        rightSelect.SetUp();
    }

    public void WhichGate(float xPos, GameObject target)
    {
        if (xPos < 0) // 왼쪽 게이트
        {
            if (leftSelect != null)
            {
                leftSelect.ActivateEffect(target);
            }
        }
        else // 오른쪽 게이트
        {
            if (rightSelect != null)
            {
                rightSelect.ActivateEffect(target);
            }
        }
    }

}
