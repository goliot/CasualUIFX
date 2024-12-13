using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header ("# DistanceBar")]
    public Slider distanceSlider;
    public TextMeshProUGUI distanceText;
    public float speed = 3f; // √ º” ~m
    private PlayerMovement player;
    public float targetValue = 30f;
    private float startValue = 0f;

    [Header("# HpBar")]
    [SerializeField]
    private GameObject hpBarPrefab;
    [SerializeField]
    private GameObject playerHpbarPrefab;

    [Header("# DamagePopup")]
    [SerializeField]
    private GameObject damagePrefab;

    [Header("# Shop")]
    [SerializeField]
    private GameObject shopUI;

    [Header("# Coin")]
    [SerializeField]
    private TextMeshProUGUI coinText;

    private void Start()
    {
        player = GameManager.instance.player.GetComponentInParent<PlayerMovement>();
        distanceSlider.minValue = startValue;
        distanceSlider.maxValue = targetValue;
        distanceSlider.value = startValue;
    }

    private void Update()
    {
        if(player.isMoving && player.speed != 0)
        {
            float step = speed * Time.deltaTime;
            distanceSlider.value = Mathf.MoveTowards(distanceSlider.value, targetValue, step);
        }
        distanceText.text = ((int)distanceSlider.value).ToString() + "M";
        coinText.text = player.gameObject.GetComponentInChildren<PlayerController>().coin.ToString();
    }

    public GameObject MakeHpBar(PlayerController player)
    {
        GameObject hpBar = Instantiate(playerHpbarPrefab, player.gameObject.transform.position, Quaternion.identity, transform);
        hpBar.GetComponent<HpBar>().player = player;
        hpBar.GetComponent<HpBar>().isMonster = false;

        return hpBar;
    }

    public GameObject MakeHpBar(Monster monster)
    {
        GameObject hpBar = Instantiate(hpBarPrefab, monster.gameObject.transform.position, Quaternion.identity, transform);
        hpBar.GetComponent<HpBar>().monster = monster;
        hpBar.GetComponent<HpBar>().isMonster = true;

        return hpBar;
    }

    public GameObject MakeDamagePopup(Transform target, float damage)
    {
        GameObject popUp = Instantiate(damagePrefab, target.transform.position, Quaternion.identity, transform);

        return popUp;
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
        GameManager.instance.Stop(true);
    }
}
