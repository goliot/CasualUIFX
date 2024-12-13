using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnBlade : MonoBehaviour
{
    public float damage = 10;

    public Transform player; // 플레이어를 참조
    public float orbitRadius; // 플레이어와의 거리 (공전 반지름)
    public float orbitSpeed; // 공전 속도
    public float rotationSpeed; // 자전 속도

    public int bladeIndex; // 표창의 고유 인덱스 (몇 번째 표창인지)
    public int totalBlades; // 현재 궤도의 총 표창 개수

    private float angleOffset; // 각 표창마다 시작 각도

    private void Start()
    {
        player = GameManager.instance.player.gameObject.transform;
    }

    public void UpdateBladePosition()
    {
        // 각도를 다시 계산
        angleOffset = (360f / totalBlades) * bladeIndex;
    }

    void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        // 1. 자전: 표창이 자신의 축을 기준으로 회전
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 2. 공전: 시간에 따른 각도 + 시작 각도를 이용해 플레이어 주변으로 공전
        float currentAngle = (orbitSpeed * Time.time) + angleOffset;
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius; // X 좌표
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius; // Y 좌표

        // 표창의 위치를 플레이어 주변으로 이동시킴
        transform.position = new Vector3(player.position.x + x, player.position.y + y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {   
            if(!GameManager.instance.player.GetComponent<PlayerController>().canMove)
                return;
            GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.Blade);

            collision.gameObject.GetComponent<Monster>().TakeDamage(damage);
        }
    }
}
