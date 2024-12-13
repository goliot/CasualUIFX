using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement parentScript;
    [SerializeField]
    private string attackTrigger = "OnAttack";
    [SerializeField]
    private Animator swordAnim;
    [SerializeField]
    private float invulnerabilityDuration = 0.5f;
    public bool isInvulnerable = false;

    [Header("# Stats")]
    public float maxHealth;
    public float health;
    public float attackSpeed;
    public float damage;
    public int coin;

    public List<GameObject> enemys;
    public bool canMove = true;

    public float detectionDistance = 2f; // 전방 감지 거리, 더 짧게 설정
    public float detectionHeight = 0.1f; // 전방 박스의 높이, 매우 짧게 설정

    private Animator animator;
    private Coroutine coBasicAttack;
    private SpriteRenderer spriteRenderer;
    private AddOnManager addOnManager;
    public BoxCollider2D boxCollider;
    private Color originColor;
    private Color hitColor = Color.red;

    private bool onColBar = false;

    private void Awake()
    {
        addOnManager = GetComponent<AddOnManager>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        parentScript = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void Start()
    {
        GameManager.instance.uiManager.MakeHpBar(this);
    }

    #region 기본 공격 관련
    private void BasicAttack(Monster enemy)
    {
        if (enemys.Count > 0)
        {
            if (enemy.health - damage <= 0)
            {
                enemy.TakeDamage(damage);
                enemys.Remove(enemy.gameObject);
                //GameManager.instance.poolManager.Release(enemy.gameObject);
            }
            else
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private GameObject FindClosest()
    {
        GameObject closest = null;

        foreach (GameObject obj in enemys)
        {
            if (closest == null)
                closest = obj;
            else
            {
                if (Vector2.Distance(transform.position, obj.transform.position) < Vector2.Distance(transform.position, closest.transform.position))
                {
                    closest = obj;
                }
            }
        }

        return closest;
    }

    IEnumerator CoBasicAttack()
    {
        while (enemys.Count > 0)
        {
            if (!canMove) continue;
            animator.SetTrigger(attackTrigger);
            GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.BasicAtk);

            //swordAnim.SetTrigger(attackTrigger);
            Monster enemy = FindClosest().GetComponent<Monster>();
            BasicAttack(enemy);
            Debug.Log("Count : " + enemys.Count);

            if (enemys.Count > 0) //아직 적이 남아있다면 피격 후 0.5초 무적
            {
                TakeDamage(enemy.damage);
            }
            yield return new WaitForSeconds(attackSpeed);
        }

        // 모든 적이 제거되면 코루틴을 종료
        coBasicAttack = null;
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.GetHit);
        health -= damage;
        GameManager.instance.cameraController.TriggerShake(); // 카메라 진동
        GameObject damagePopup = GameManager.instance.poolManager.Get("DamagePopup");
        damagePopup.GetComponent<DamagePopup>().damageText.text = ((int)damage).ToString();
        damagePopup.GetComponent<DamagePopup>().master = transform;
        damagePopup.GetComponent<DamagePopup>().Setup(damage);

        if(health <= 0)
        {
            Die();
            GameManager.instance.GameOver();
            return;
        }

        StartCoroutine(BecomeInvulnerable());
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

    public LayerMask enemyLayer;       // 적이 속한 레이어

    private void Update()
    {
        // 전방 방향을 기준으로 박스 콜라이더 설정
        Vector2 boxCenter = (Vector2)transform.position + (Vector2)(boxCollider.offset * transform.localScale) + (Vector2.up * (detectionDistance / 2));
        Vector2 boxSize = new Vector2(boxCollider.size.x * transform.localScale.x, detectionHeight);

        // 박스 범위 내에서 적을 감지
        Collider2D[] detectedEnemies = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);

        // 적 리스트 업데이트
        enemys.Clear();
        foreach (Collider2D enemy in detectedEnemies)
        {
            enemys.Add(enemy.gameObject);
        }

        // 적이 있으면 공격 시작
        if (enemys.Count > 0)
        {
            parentScript.ChangeMoveState(false);
            if (coBasicAttack == null)
            {
                coBasicAttack = StartCoroutine(CoBasicAttack());
            }
        }
        else
        {
            if(!onColBar) parentScript.ChangeMoveState(true);
            /*if (coBasicAttack != null)
            {
                StopCoroutine(coBasicAttack);
                coBasicAttack = null;
            }*/
        }
    }

    // 시각적으로 박스 범위 확인용
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 전방 박스 콜라이더의 중심과 크기를 계산
        Vector2 boxCenter = (Vector2)transform.position + (Vector2)(boxCollider.offset * transform.localScale) + (Vector2.up * (detectionDistance / 2));
        Vector2 boxSize = new Vector2(boxCollider.size.x * transform.localScale.x, detectionHeight);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision) // 두번 호출은 되고있음
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Bar") && parentScript != null)
        {
            parentScript.ChangeMoveState(false);
            onColBar = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bar") && parentScript != null)
        {
            parentScript.ChangeMoveState(true);
            onColBar = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            collision.gameObject.GetComponentInParent<Gate>().WhichGate(transform.position.x, gameObject);
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
    }
}
