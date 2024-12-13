using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private float spawnProbability;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private float gateSpawnTime = 60f;
    [SerializeField]
    private float gateSpawnAmount = 5f;

    private float curTime;
    private float gateCurTime;
    public bool canSpawn = true;

    public List<string> nameToSpawn;
    public List<float> spawnProbabilities; // 각 개체의 스폰 확률을 정의하는 리스트

    private void Update()
    {
        curTime += Time.deltaTime;
        gateCurTime += Time.deltaTime;

        if (!canSpawn) return;

        float distanceSliderValue = GameManager.instance.uiManager.distanceSlider.value;
        if (Mathf.Abs(distanceSliderValue - GameManager.instance.uiManager.distanceSlider.maxValue) < 10f)
        {
            SpawnBoss();
            return;
        }
        else if ((int)distanceSliderValue % 50 == 0 && (int)distanceSliderValue != 0)
        {
            if (--gateSpawnAmount < 0) return;
            Debug.Log((int)distanceSliderValue % 50);
            SpawnGate();
            gateCurTime = 0;
            return;
        }
        else if (curTime > spawnTime && GameManager.instance.player.GetComponentInParent<PlayerMovement>().isMoving)
        {
            if (Mathf.Abs(distanceSliderValue - GameManager.instance.uiManager.distanceSlider.maxValue) < 20)
                return;
            Spawn();
            curTime = 0;
            spawnTime = Random.Range(0.5f, 2f);
            return;
        }
    }

    private void Spawn()
    {
        foreach (Transform t in spawnPoints)
        {
            if (GetRandomBool(spawnProbability))
            {
                int index = GetWeightedRandomIndex();
                Debug.Log("index : " + index);
                GameObject obj = GameManager.instance.poolManager.Get(nameToSpawn[index]);
                obj.transform.position = new Vector3(t.position.x, t.position.y + 10, 0);

                // Coin의 초기 위치를 설정
                if (nameToSpawn[index] == "Coin")
                {
                    Coin coin = obj.GetComponent<Coin>();
                    if (coin != null)
                    {
                        coin.SetInitialPosition(obj.transform.position);
                    }
                }

                obj.transform.rotation = Quaternion.identity;
            }
        }
    }

    private void SpawnGate()
    {
        canSpawn = false;
        GameObject gate = GameManager.instance.poolManager.Get("Gate");
        gate.transform.position = new Vector3(0, spawnPoints[0].transform.position.y + 10f, 0);
        Invoke("EnableSpawn", 1f);
    }

    private void SpawnBoss()
    {
        canSpawn = false;
        GameObject boss = GameManager.instance.poolManager.Get("Boss");
        boss.transform.position = new Vector3(0, spawnPoints[0].transform.position.y + 10f, 0);


        GameObject goal = GameManager.instance.poolManager.Get("Goal");
        goal.transform.position = boss.transform.position + new Vector3(0, 10f, 0);
    }

    public GameObject SpawnFireBallRain()
    {
        int idx = Random.Range(0, spawnPoints.Length);
        GameObject fireBall = GameManager.instance.poolManager.Get("RedBullet");
        fireBall.transform.position = new Vector3(spawnPoints[idx].transform.position.x, spawnPoints[idx].transform.position.y, 0);
        return fireBall;
    }

    private void EnableSpawn()
    {
        canSpawn = true;
    }

    private bool GetRandomBool(float trueProbability)
    {
        return Random.value < trueProbability;
    }

    private int GetWeightedRandomIndex()
    {
        float total = 0;
        foreach (float prob in spawnProbabilities)
        {
            total += prob;
        }

        float randomValue = Random.value * total;
        float runningTotal = 0;

        for (int i = 0; i < spawnProbabilities.Count; i++)
        {
            runningTotal += spawnProbabilities[i];
            if (randomValue < runningTotal)
            {
                return i;
            }
        }

        return spawnProbabilities.Count - 1; // Fallback
    }
}
