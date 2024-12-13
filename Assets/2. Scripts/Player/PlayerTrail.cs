using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Start()
    {
        // Trail Renderer 컴포넌트 가져오기
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer 컴포넌트가 없습니다.");
        }

        Color startColor = new Color(0.5f, 0.5f, 0.5f, 1f); // 중간 회색, 불투명
        Color endColor = new Color(0.5f, 0.5f, 0.5f, 0f);   // 중간 회색, 투명
        trailRenderer.startColor = startColor;
        trailRenderer.endColor = endColor;
    }

    private void Update()
    {
        if (!trailRenderer.enabled)
            trailRenderer.enabled = true;
    }
}
