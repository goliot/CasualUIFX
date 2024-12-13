using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // For using LINQ methods

public class Shop : MonoBehaviour
{
    public List<GateEffects> effects; // ��ü ȿ�� ����Ʈ
    private List<GateEffects> effectsThisTime = new List<GateEffects>(); // �̹��� ���õ� ȿ�� ����Ʈ

    public GameObject[] buttons; // ��ư �迭

    void OnEnable()
    {
        StartCoroutine(ScalePopup());

        effects = GameManager.instance.effects;
        Reroll(false); // ���� Ȱ��ȭ�� �� �� �� ����
        GameManager.instance.Stop(true);
    }

    private void OnDisable()
    {
        GameManager.instance.Stop(false);
    }

    public void Reroll(bool isBtn)
    {
        if (isBtn)
        {
            GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.BtnClick);

            if (GameManager.instance.player.GetComponent<PlayerController>().coin < 2) return;

            GameManager.instance.player.GetComponent<PlayerController>().coin -= 2;
        }

        effectsThisTime.Clear();

        effectsThisTime = effects
                    .Where(e => e.canBeInShop)
                    .OrderBy(e => Random.value)
                    .Take(6)
                    .ToList();

        for (int i = 0; i < buttons.Length && i < effectsThisTime.Count; i++)
        {
            buttons[i].GetComponent<ShopBtn>().effect = effectsThisTime[i];
            buttons[i].GetComponent<ShopBtn>().desc.text = effectsThisTime[i].desc;
            buttons[i].GetComponent<ShopBtn>().price.text = effectsThisTime[i].price.ToString();
        }
    }

    private IEnumerator ScalePopup()
    {
        float duration = 1f; // �ִϸ��̼��� ����Ǵ� �� �ð�
        float elapsed = 0f;  // ��� �ð�

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(0.7f, 0.7f, 0.7f);      

        // ������ �ִϸ��̼� ����
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 0���� 1 ������ ���� ��
            float t = Mathf.Clamp01(elapsed / duration);

            // EaseOutBounce�� �����Ͽ� ������ �� ���
            float easedT = EaseOutBounce(t);

            // �������� easedT�� ���� ����
            transform.localScale = Vector3.Lerp(startScale, endScale, easedT);

            yield return null; // ���� �����ӱ��� ���
        }

        // ���� �������� ��Ȯ�� 1�� ����
        transform.localScale = endScale;
    }

    private float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
}
