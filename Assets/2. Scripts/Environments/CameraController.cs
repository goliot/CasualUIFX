using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    public bool isStopped = false;

    [Header("# CameraShake")]
    public float shakeDuration = 0.1f; // 흔들리는 시간
    public float shakeMagnitude = 2.0f; // 흔들림의 세기 (각도)
    public float dampingSpeed = 1.0f; // 흔들림이 줄어드는 속도

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        isStopped = false;
    }

    private void Update()
    {
        if (isStopped) return;
        transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
    }

    public void TriggerShake()
    {
        StartCoroutine(CoShake());
    }

    private IEnumerator CoShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float xAngle = Random.Range(-1f, 1f) * shakeMagnitude;
            float yAngle = Random.Range(-1f, 1f) * shakeMagnitude;

            Quaternion randomRotation = Quaternion.Euler(xAngle, yAngle, 0f);
            transform.localRotation = initialRotation * randomRotation;

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 흔들림이 끝나면 원래 회전 값으로 복원
        transform.localRotation = initialRotation;
    }
}
