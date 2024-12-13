using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리팹 보관 변수
    public GameObject[] prefabs;
    public enum poolObjects
    {
        GreenSlime = 0,
        Blade = 1,
        Bird = 2,
        BirdBullet = 3,
        Shuriken = 4,
        Gate = 5,
        Coin = 6,
        RedSlime = 7,
        RedBullet = 8,
        Explode = 9,
        Boss = 10,
        Goal = 11,
        DamagePopup = 12
    };

    //풀 담당 리스트
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// 선택한 풀의 놀고 있는(비활성화된) 게임 오브젝트 접근
    /// 발견하면 select에 할당
    /// 못찾으면 새로 생성
    /// </summary>
    public GameObject GetFromPool(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }

    public poolObjects StringToPoolObjects(string str) //string -> enum 변환 함수
    {
        return (poolObjects)System.Enum.Parse(typeof(poolObjects), str);
    }

    public GameObject Get(string type) //string으로 소환하고 싶은 것을 받아오면, 그에 맞는 enum으로 바꿔 Get 함수로 전달
    {
        return GetFromPool((int)StringToPoolObjects(type));
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}
