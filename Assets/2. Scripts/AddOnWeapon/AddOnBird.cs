using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnBird : MonoBehaviour
{
    private Vector2 offset = new Vector2(-0.5f, -0.7f); // 2D에서는 Vector2 사용
    public float bobbingAmplitude = 0.2f; // 흔들림의 강도
    public float bobbingSpeed = 4f;       // 흔들림의 속도
    private float initialY;               // 초기 Y 좌표값

    [Header("# Stats")]
    public float damage = 1f;
    public float attackSpeed = 1f;

    private float curTime = 0f;

    // 따라가는 속도
    public float followSmoothness = 0.1f;  // 부드럽게 따라가는 정도 (값이 작을수록 더 천천히 따라감)
    private Vector2 velocity = Vector2.zero; // SmoothDamp에 사용할 속도 값

    private void Start()
    {
        initialY = offset.y;
    }

    private void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;
        float newY = initialY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude;
        Vector2 bobbingOffset = new Vector2(offset.x, newY);

        Vector2 targetPosition = (Vector2)GameManager.instance.player.transform.position + bobbingOffset;

        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothness);

        curTime += Time.deltaTime;
        if (curTime > attackSpeed)
        {
            Shoot();
            curTime = 0f;
        }
    }

    private void Shoot()
    {
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.Bird);

        GameObject bullet = GameManager.instance.poolManager.Get("BirdBullet");
        bullet.GetComponent<BirdBullet>().transform.position = transform.position + bullet.GetComponent<BirdBullet>().offset;
        bullet.GetComponent<BirdBullet>().damage = damage;
    }
}
