using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    public float attractRange = 2f;
    public float attractSpeed = 5f;
    private bool isAttracting = false;
    private Vector3 initialPosition;
    private Vector3 controlPoint;
    private float t = 0f;
    private Vector3 originalPosition;
    public float floatAmplitude = 0.2f;
    public float floatSpeed = 1f;
    public float maxDistance = 30f;

    private bool isSet = false;

    private void Awake()
    {
        player = GameManager.instance.player.gameObject.transform;
    }

    private void OnEnable()
    {
        isSet = false;
        isAttracting = false;
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
        originalPosition = position; // 초기화 시 원래 위치 설정
        isSet = true;
    }

    void Update()
    {
        if (!isSet) return;
        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > maxDistance)
        {
            GameManager.instance.poolManager.Release(gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attractRange && !isAttracting)
        {
            // Control point를 계산할 때, y축의 범위를 더 제한함
            float randomXOffset = Random.Range(-0.5f, 0.5f);
            float randomYOffset = Random.Range(1f, 3f); // Y축의 오프셋 조정
            controlPoint = (initialPosition + player.position) / 2 + new Vector3(randomXOffset, randomYOffset, 0);
            isAttracting = true;
            t = 0f;
        }

        if (isAttracting)
        {
            MoveAlongCurve();
        }
        else
        {
            FloatEffect();
        }
    }

    void MoveAlongCurve()
    {
        t += attractSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        transform.position = CalculateBezierPoint(t, initialPosition, controlPoint, player.position);

        if (Vector3.Distance(transform.position, player.position) < 0.1f)
        {
            player.gameObject.GetComponent<PlayerController>().coin++;
            GameManager.instance.poolManager.Release(gameObject);
        }
    }

    void FloatEffect()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = originalPosition + new Vector3(0, yOffset, 0);
    }

    Vector3 CalculateBezierPoint(float t, Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
    {
        return Mathf.Pow(1 - t, 2) * startPoint + 2 * (1 - t) * t * controlPoint + Mathf.Pow(t, 2) * endPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRange);
    }
}
