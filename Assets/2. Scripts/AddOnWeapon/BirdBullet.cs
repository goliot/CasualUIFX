using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBullet : MonoBehaviour
{
    public float damage;
    public float speed;

    public float maxDistance = 40f;

    public Vector3 offset = new Vector3(0, 0.5f, 0);

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        transform.position += Vector3.up * speed * Time.deltaTime;

        if(Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > maxDistance)
        {
            GameManager.instance.poolManager.Release(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Monster") return;

        collision.gameObject.GetComponent<Monster>().TakeDamage(damage);
        GameManager.instance.poolManager.Release(gameObject);
    }
}
