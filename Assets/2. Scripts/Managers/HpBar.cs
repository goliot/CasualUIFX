using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Slider hpBar;

    private float maxHp;
    private float curHp;

    public bool isMonster;
    public Monster monster; // 이 hp바의 주인
    public PlayerController player;
    public TextMeshProUGUI playerHpText;

    private Camera cam;

    private void OnEnable()
    {
        hpBar = GetComponent<Slider>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!monster && !player) return;

        if(isMonster)
        {
            transform.position = cam.WorldToScreenPoint(monster.gameObject.transform.position + new Vector3(0, 0.2f, 0));
            maxHp = monster.maxHealth;
            curHp = monster.health;
        }
        else
        {
            transform.position = cam.WorldToScreenPoint(player.gameObject.transform.position + new Vector3(0, -0.7f, 0));
            maxHp = player.maxHealth;
            curHp = player.health;
            if (player.health < 0)
                playerHpText.text = "0";
            else 
                playerHpText.text = player.health.ToString();
        }
        HandleHp();
    }

    private void HandleHp()
    {
        //hpBar.value = curHp / maxHp;
        hpBar.value = Mathf.Lerp(hpBar.value, curHp / maxHp, Time.deltaTime * 10);
    }
}
