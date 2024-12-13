using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public Transform master; // 팝업의 주인
    public TextMeshPro damageText;
    public float popupDuration = 1f;
    private Vector3 startPosition; // 초기 월드 좌표 위치
    private Vector3 targetPosition; // 목표 위치
    private Transform masterParent; // 마스터의 부모 (플레이어의 경우)

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        // 팝업이 다시 활성화될 때마다 투명도를 최대치로 설정
        Color color = damageText.color;
        color.a = 1f;
        damageText.color = color;
    }

    public void Setup(float damage)
    {
        // 마스터의 부모를 기준으로 위치 설정 (플레이어의 경우에만)
        if (master.gameObject.tag == "Player")
        {
            masterParent = master.parent;
            startPosition = masterParent.position + new Vector3(Random.Range(-0.3f, 0.3f), -3.3f, 0);
            damageText.color = Color.red;
        }
        else
        {
            startPosition = master.position + new Vector3(Random.Range(-0.3f, 0.3f), 0.3f, 0);
            damageText.color = Color.yellow;
        }

        targetPosition = startPosition; // 초기 목표 위치 설정
        damageText.text = ((int)damage).ToString();
        transform.position = startPosition; // 팝업의 초기 위치 설정
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float time = 0;
        Vector3 initialPosition = transform.position; // 초기 월드 좌표

        while (time < popupDuration)
        {
            time += Time.deltaTime;
            float progress = time / popupDuration;

            // 투명도 계산 (점차 1에서 0으로 감소)
            float alpha = Mathf.Lerp(1f, 0f, progress);

            // 투명도 적용
            Color color = damageText.color;
            color.a = alpha;
            damageText.color = color;

            // 목표 위치 갱신
            if (master.gameObject.tag == "Player")
            {
                // 플레이어의 현재 위치를 기준으로 목표 위치 갱신
                targetPosition = masterParent.position + new Vector3(startPosition.x, -3.8f, 0);
                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            }
            else
            {
                // Y축 이동에 EaseOutBounce 함수 적용 (월드 좌표에서)
                float bounceValue = EaseOutBounce(progress);
                transform.position = initialPosition + new Vector3(0, bounceValue * 0.3f, 0); // 월드 좌표에서 Y축 이동
            }

            yield return null;
        }

        GameManager.instance.poolManager.Release(gameObject); // 애니메이션 종료 후 객체 제거
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
