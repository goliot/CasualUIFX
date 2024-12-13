using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float damage;
    public float speed;
    public float maxDistance = 40f;

    private Vector2 direction; // 발사 방향 저장

    private void OnEnable()
    {
        direction = Vector2.up; 
    }

    private void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > maxDistance)
        {
            GameManager.instance.poolManager.Release(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Monster") return;

        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.Shuriken);

        collision.gameObject.GetComponent<Monster>().TakeDamage(damage);
        GameManager.instance.poolManager.Release(gameObject);
    }
}
