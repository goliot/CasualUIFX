using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPopup : MonoBehaviour
{
    public TextMeshProUGUI recordText;

    private void OnEnable()
    {
        // 팝업이 활성화될 때 스케일 애니메이션 시작
        StartCoroutine(ScalePopup());
    }

    private IEnumerator ScalePopup()
    {
        float duration = 1f; // 애니메이션이 진행되는 총 시간
        float elapsed = 0f;  // 경과 시간

        Vector3 startScale = Vector3.zero;    // 초기 스케일 (0)
        Vector3 endScale = Vector3.one;       // 최종 스케일 (1)

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

    public void OnClickRe()
    {
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.BtnClick);

        SceneManager.LoadScene(0);
    }
}
