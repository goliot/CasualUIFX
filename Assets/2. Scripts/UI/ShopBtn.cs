using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtn : MonoBehaviour
{
    public GateEffects effect;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI price;

    private void OnEnable()
    {
        GetComponent<Button>().enabled = true;
    }

    public void OnClickThis()
    {
        GameManager.instance.audioManager.PlaySfx(AudioManager.Sfx.BtnClick);
        if (GameManager.instance.player.GetComponent<PlayerController>().coin < int.Parse(price.text))
        {
            return;
        }
        GameManager.instance.player.GetComponent<PlayerController>().coin -= int.Parse(price.text);
        effect.ApplyEffect(GameManager.instance.player);
        GetComponent<Button>().enabled = false;
    }
}
