using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnManager : MonoBehaviour
{
    [Header("# AddOn")]
    public bool isBirdOn = false;
    public bool isBladeOn = false;
    public bool isShurikenOn = false;

    public List<List<GameObject>> blades = new List<List<GameObject>>(); // �� �˵��� ���̵� ����Ʈ
    private PlayerController playerController;

    [Header("# Shuriken Settings")]
    public int maxShurikenCount = 6; // �ִ� �߻� ������ ����
    public float spreadAngle = 10f; // �¿� ���� ����
    private float curTime = 0;
    public float shurikenTime = 1f;
    private int shurikenCount = 0;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!GameManager.instance.player.GetComponent<PlayerController>().canMove)
            return;

        curTime += Time.deltaTime;
        if(isShurikenOn && curTime > shurikenTime)
        {
            ShootShuriken();
            curTime = 0;
        }
    }

    #region Blade
    public void AddBlade(int index) // �ε��� = ���° �˵��� �߰��ϴ���
    {
        if (blades.Count == 0) // ù ��° �˵� �߰�
        {
            blades.Add(new List<GameObject>());
            isBladeOn = true;
        }

        if (blades.Count <= index) return; // �������� �ʴ� �˵��� �߰��ϴ� ��� ����

        // �� ǥâ ����
        GameObject blade = GameManager.instance.poolManager.Get("Blade");
        blade.GetComponent<AddOnBlade>().bladeIndex = blades[index].Count;
        blade.GetComponent<AddOnBlade>().orbitRadius = index + 1; // ������ ����
        blade.GetComponent<AddOnBlade>().totalBlades = blades[index].Count + 1; // �� ���̵� �� ����
        blades[index].Add(blade);

        // ��� ���̵� ��ġ ������
        UpdateBladesPosition(index);
    }

    public void AddOuterBlade() // ���ο� �˵� �߰�
    {
        blades.Add(new List<GameObject>());
        AddBlade(blades.Count - 1); // ���� �ٱ��ʿ� ���ο� ���̵� �߰�
    }

    // �� �˵��� �ִ� ���̵���� ��ġ�� ������
    private void UpdateBladesPosition(int index)
    {
        for (int i = 0; i < blades[index].Count; i++)
        {
            GameObject blade = blades[index][i];
            AddOnBlade bladeScript = blade.GetComponent<AddOnBlade>();

            bladeScript.bladeIndex = i;
            bladeScript.totalBlades = blades[index].Count;
            bladeScript.UpdateBladePosition(); // ������ �ٽ� ����
        }
    }
    #endregion

    #region Bird
    public void AddBird()
    {
        if (isBirdOn) return;

        isBirdOn = true;
        GameManager.instance.poolManager.Get("Bird");
    }
    #endregion

    #region Shuriken
    public void AddShuriken()
    {
        if(!isShurikenOn) isShurikenOn = true;
        if(maxShurikenCount > shurikenCount) shurikenCount++;
    }

    public void ShootShuriken()
    {
        if (shurikenCount == 1)
        {
            // �������� �ϳ��� ���, �ٷ� ���� �߻�
            GameObject shuriken = GameManager.instance.poolManager.Get("Shuriken");
            shuriken.transform.position = GameManager.instance.player.transform.position;
            shuriken.transform.rotation = Quaternion.Euler(0, 0, 90);

            shuriken.GetComponent<Shuriken>().SetDirection(Vector2.up);
        }
        else
        {
            // ������ ����� ���� �� �߻�
            float angleStep = spreadAngle * 2 / (shurikenCount - 1);
            float startAngle = -spreadAngle;

            for (int i = 0; i < shurikenCount; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

                GameObject shuriken = GameManager.instance.poolManager.Get("Shuriken");
                shuriken.transform.position = GameManager.instance.player.transform.position;
                shuriken.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

                shuriken.GetComponent<Shuriken>().SetDirection(direction);
            }
        }
    }

    #endregion
}
