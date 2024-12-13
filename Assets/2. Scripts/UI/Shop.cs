using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // For using LINQ methods

public class Shop : MonoBehaviour
{
    public List<GateEffects> effects; // 전체 효과 리스트
    private List<GateEffects> effectsThisTime = new List<GateEffects>(); // 이번에 선택된 효과 리스트

    public GameObject[] buttons; // 버튼 배열

    void OnEnable()
    {
        StartCoroutine(ScalePopup());

        effects = GameManager.instance.effects;
        Reroll(false); // 상점 활성화될 때 한 번 리롤
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
        float duration = 1f; // 애니메이션이 진행되는 총 시간
        float elapsed = 0f;  // 경과 시간

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(0.7f, 0.7f, 0.7f);      

        // 스케일 애니메이션 진행
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 0에서 1 사이의 보간 값
            float t = Mathf.Clamp01(elapsed / duration);

            // EaseOutBounce를 적용하여 스케일 값 계산
            float easedT = EaseOutBounce(t);

            // 스케일을 easedT에 따라 보간
            transform.localScale = Vector3.Lerp(startScale, endScale, easedT);

            yield return null; // 다음 프레임까지 대기
        }

        // 최종 스케일을 정확히 1로 설정
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
