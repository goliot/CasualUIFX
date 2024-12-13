using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnBlade : MonoBehaviour
{
    public float damage = 10;

    public Transform player; // �÷��̾ ����
    public float orbitRadius; // �÷��̾���� �Ÿ� (���� ������)
    public float orbitSpeed; // ���� �ӵ�
    public float rotationSpeed; // ���� �ӵ�

    public int bladeIndex; // ǥâ�� ���� �ε��� (�� ��° ǥâ����)
    public int totalBlades; // ���� �˵��� �� ǥâ ����

    private float angleOffset; // �� ǥâ���� ���� ����

    private void Start()
    {
        player = GameManager.instance.player.gameObject.transform;
    }

    public void UpdateBladePosition()
    {
        // ������ �ٽ� ���
        angleOffset = (360f / totalBlades) * bladeIndex;
    }

    void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        // 1. ����: ǥâ�� �ڽ��� ���� �������� ȸ��
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 2. ����: �ð��� ���� ���� + ���� ������ �̿��� �÷��̾� �ֺ����� ����
        float currentAngle = (orbitSpeed * Time.time) + angleOffset;
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius; // X ��ǥ
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius; // Y ��ǥ

        // ǥâ�� ��ġ�� �÷��̾� �ֺ����� �̵���Ŵ
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
