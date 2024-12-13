using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager poolManager;
    public Spawner spawner;
    public GameObject player;
    public CameraController cameraController;
    public UIManager uiManager;
    public AudioManager audioManager;
    public Background[] background;
    public List<GateEffects> effects;

    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private GameObject victoryEffect;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject gameClearUI;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    private void Start()
    {
        audioManager.PlayBgm(true);
        victoryEffect.SetActive(false);
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
    }

    public void GameOver()
    {
        StartCoroutine(CoDefeat());
        gameOverUI.SetActive(true);
        audioManager.PlaySfx(AudioManager.Sfx.Defeat);
    }

    public void GameClear()
    {
        StartCoroutine(CoClear());
        audioManager.PlaySfx(AudioManager.Sfx.Clear);
    }

    public void Stop(bool how)
    {
        cameraController.isStopped = how;
        if (how)
            player.GetComponentInParent<PlayerMovement>().speed = 0;
        else
            player.GetComponentInParent<PlayerMovement>().speed = 3f;
        player.GetComponent<PlayerController>().isInvulnerable = how;
        for (int i = 0; i < background.Length; i++)
        {
            background[i].isStopped = how;
        }
        spawner.canSpawn = !how;
        player.GetComponent<PlayerController>().canMove = !how;
    }

    IEnumerator CoClear()
    {
        yield return new WaitForSeconds(1f);
        //cam.GetComponent<CameraController>().isStopped = true;
        player.GetComponentInParent<PlayerMovement>().speed = 10f;
        PlayerPrefs.SetInt("BestRecord", (int)uiManager.distanceSlider.value);
        yield return new WaitForSeconds(0.5f);

        victoryEffect.SetActive(true);
        gameClearUI.SetActive(true);
        gameClearUI.GetComponent<GameEndPopup>().recordText.text = "Best : " + PlayerPrefs.GetInt("BestRecord").ToString() + "M";
        uiManager.distanceSlider.gameObject.SetActive(false);
    }

    IEnumerator CoDefeat()
    {
        yield return new WaitForSeconds(1f);
        Stop(true);
        gameOverUI.SetActive(true);
        PlayerPrefs.SetInt("Best Record", (int)uiManager.distanceSlider.value);
        gameObject.GetComponent<GameEndPopup>().recordText.text = "Best : " + PlayerPrefs.GetInt("BestRecord").ToString() + "M";
        uiManager.distanceSlider.gameObject.SetActive(false);
    }
}
