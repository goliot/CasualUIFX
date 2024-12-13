using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPopup : MonoBehaviour
{
    public TextMeshProUGUI recordText;

    private void OnEnable()
    {
        // �˾��� Ȱ��ȭ�� �� ������ �ִϸ��̼� ����
        StartCoroutine(ScalePopup());
    }

    private IEnumerator ScalePopup()
    {
        float duration = 1f; // �ִϸ��̼��� ����Ǵ� �� �ð�
        float elapsed = 0f;  // ��� �ð�

        Vector3 startScale = Vector3.zero;    // �ʱ� ������ (0)
        Vector3 endScale = Vector3.one;       // ���� ������ (1)

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

    public void OnClickRe()
    {
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.BtnClick);

        SceneManager.LoadScene(0);
    }
}
