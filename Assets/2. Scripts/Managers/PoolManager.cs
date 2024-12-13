using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //������ ���� ����
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

    //Ǯ ��� ����Ʈ
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
    /// ������ Ǯ�� ��� �ִ�(��Ȱ��ȭ��) ���� ������Ʈ ����
    /// �߰��ϸ� select�� �Ҵ�
    /// ��ã���� ���� ����
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

    public poolObjects StringToPoolObjects(string str) //string -> enum ��ȯ �Լ�
    {
        return (poolObjects)System.Enum.Parse(typeof(poolObjects), str);
    }

    public GameObject Get(string type) //string���� ��ȯ�ϰ� ���� ���� �޾ƿ���, �׿� �´� enum���� �ٲ� Get �Լ��� ����
    {
        return GetFromPool((int)StringToPoolObjects(type));
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}
