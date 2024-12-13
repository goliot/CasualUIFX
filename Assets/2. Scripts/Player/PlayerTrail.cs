using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Start()
    {
        // Trail Renderer ������Ʈ ��������
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer ������Ʈ�� �����ϴ�.");
        }

        Color startColor = new Color(0.5f, 0.5f, 0.5f, 1f); // �߰� ȸ��, ������
        Color endColor = new Color(0.5f, 0.5f, 0.5f, 0f);   // �߰� ȸ��, ����
        trailRenderer.startColor = startColor;
        trailRenderer.endColor = endColor;
    }

    private void Update()
    {
        if (!trailRenderer.enabled)
            trailRenderer.enabled = true;
    }
}
