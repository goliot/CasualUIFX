using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public enum MonsterType
{
    Green,
    Red,
    Boss,
}
public class Monster : MonoBehaviour
{
    [SerializeField]
    private float maxDistance;

    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField]
    private float knockbackForce = 10f;

    [SerializeField]
    private float knockbackDistance = 5f;

    [SerializeField]
    private float rotationSpeed = 180f;

    [Header("# Stat")]
    public float maxHealth;
    public float health;
    public float damage;
    public MonsterType monsterType;
    public float atkSpeed;

    private float invulnerabilityDuration = 0.5f;
    private bool isInvulnerable = false;

    private Vector2 initialPosition;
    private bool isFlying;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Color originColor;
    private Color hitColor = Color.red;

    private GameObject hpBar;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        anim.speed = 1;
        health = maxHealth;
        isFlying = false;
        coll.enabled = true;
        FreezeXPosition(true);
        FreezeYPosition(true);
        rb.freezeRotation = true;

        // 扁粮俊 积己等 hpBar 力芭
        if (hpBar != null)
        {
            Destroy(hpBar);
        }
    }

    public virtual void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > maxDistance)
        {
            GameManager.instance.poolManager.Release(gameObject);
        }

        if (isFlying)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            if (Vector2.Distance(initialPosition, transform.position) >= knockbackDistance)
            {
                rb.velocity = Vector2.zero;
                isFlying = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        if ((int)health == (int)maxHealth && hpBar == null)
        {
            hpBar = GameManager.instance.uiManager.MakeHpBar(this);
        }

        health -= damage;
        StartCoroutine(BecomeInvulnerable());
        GameObject damagePopUp = GameManager.instance.poolManager.Get("DamagePopup");
        damagePopUp.GetComponent<DamagePopup>().master = transform;
        damagePopUp.GetComponent<DamagePopup>().Setup(damage);

        if (health <= 0)
        {
            if (hpBar != null)
            {
                Destroy(hpBar);
                hpBar = null;
            }
            Knockback();
            coll.enabled = false;
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        StartCoroutine(ChangeColor());

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
    }

    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originColor;
    }

    private void Knockback()
    {
        anim.speed = 0;

        FreezeXPosition(false);
        FreezeYPosition(false);
        float randomAngle = Random.Range(-30f, 30f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        initialPosition = transform.position;
        rb.AddForce(randomDirection * knockbackForce, ForceMode2D.Impulse);
        isFlying = true;

        StartCoroutine(ReleaseAfterTime());
        if(monsterType == MonsterType.Boss)
        {
            GameManager.instance.GameClear();
        }
    }

    IEnumerator ReleaseAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject effect = GameManager.instance.poolManager.Get("Explode");
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.Explode);

        effect.transform.position = transform.position;
        GameManager.instance.poolManager.Release(gameObject);
    }

    public void FreezeXPosition(bool freeze)
    {
        if (freeze)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }

    public void FreezeYPosition(bool freeze)
    {
        if (freeze)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
