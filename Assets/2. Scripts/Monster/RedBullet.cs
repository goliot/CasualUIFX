using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullet : MonoBehaviour
{
    [NonSerialized]
    public float damage;
    public float speed;
    public float maxDistance = 40f;

    private void OnEnable()
    {
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.RedBullet);
    }
    private void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > maxDistance)
        {
            GameManager.instance.poolManager.Release(gameObject);
        }

        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            GameManager.instance.poolManager.Release(gameObject);
        }
    }
}
