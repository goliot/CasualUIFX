using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public Transform master; // �˾��� ����
    public TextMeshPro damageText;
    public float popupDuration = 1f;
    private Vector3 startPosition; // �ʱ� ���� ��ǥ ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ
    private Transform masterParent; // �������� �θ� (�÷��̾��� ���)

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        // �˾��� �ٽ� Ȱ��ȭ�� ������ ������ �ִ�ġ�� ����
        Color color = damageText.color;
        color.a = 1f;
        damageText.color = color;
    }

    public void Setup(float damage)
    {
        // �������� �θ� �������� ��ġ ���� (�÷��̾��� ��쿡��)
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

        targetPosition = startPosition; // �ʱ� ��ǥ ��ġ ����
        damageText.text = ((int)damage).ToString();
        transform.position = startPosition; // �˾��� �ʱ� ��ġ ����
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float time = 0;
        Vector3 initialPosition = transform.position; // �ʱ� ���� ��ǥ

        while (time < popupDuration)
        {
            time += Time.deltaTime;
            float progress = time / popupDuration;

            // ���� ��� (���� 1���� 0���� ����)
            float alpha = Mathf.Lerp(1f, 0f, progress);

            // ���� ����
            Color color = damageText.color;
            color.a = alpha;
            damageText.color = color;

            // ��ǥ ��ġ ����
            if (master.gameObject.tag == "Player")
            {
                // �÷��̾��� ���� ��ġ�� �������� ��ǥ ��ġ ����
                targetPosition = masterParent.position + new Vector3(startPosition.x, -3.8f, 0);
                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            }
            else
            {
                // Y�� �̵��� EaseOutBounce �Լ� ���� (���� ��ǥ����)
                float bounceValue = EaseOutBounce(progress);
                transform.position = initialPosition + new Vector3(0, bounceValue * 0.3f, 0); // ���� ��ǥ���� Y�� �̵�
            }

            yield return null;
        }

        GameManager.instance.poolManager.Release(gameObject); // �ִϸ��̼� ���� �� ��ü ����
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
