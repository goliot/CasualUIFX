using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnManager : MonoBehaviour
{
    [Header("# AddOn")]
    public bool isBirdOn = false;
    public bool isBladeOn = false;
    public bool isShurikenOn = false;

    public List<List<GameObject>> blades = new List<List<GameObject>>(); // 각 궤도별 블레이드 리스트
    private PlayerController playerController;

    [Header("# Shuriken Settings")]
    public int maxShurikenCount = 6; // 최대 발사 수리검 개수
    public float spreadAngle = 10f; // 좌우 퍼짐 각도
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
    public void AddBlade(int index) // 인덱스 = 몇번째 궤도에 추가하는지
    {
        if (blades.Count == 0) // 첫 번째 궤도 추가
        {
            blades.Add(new List<GameObject>());
            isBladeOn = true;
        }

        if (blades.Count <= index) return; // 존재하지 않는 궤도에 추가하는 경우 방지

        // 새 표창 생성
        GameObject blade = GameManager.instance.poolManager.Get("Blade");
        blade.GetComponent<AddOnBlade>().bladeIndex = blades[index].Count;
        blade.GetComponent<AddOnBlade>().orbitRadius = index + 1; // 반지름 설정
        blade.GetComponent<AddOnBlade>().totalBlades = blades[index].Count + 1; // 총 블레이드 수 설정
        blades[index].Add(blade);

        // 모든 블레이드 위치 재조정
        UpdateBladesPosition(index);
    }

    public void AddOuterBlade() // 새로운 궤도 추가
    {
        blades.Add(new List<GameObject>());
        AddBlade(blades.Count - 1); // 가장 바깥쪽에 새로운 블레이드 추가
    }

    // 각 궤도에 있는 블레이드들의 위치를 재조정
    private void UpdateBladesPosition(int index)
    {
        for (int i = 0; i < blades[index].Count; i++)
        {
            GameObject blade = blades[index][i];
            AddOnBlade bladeScript = blade.GetComponent<AddOnBlade>();

            bladeScript.bladeIndex = i;
            bladeScript.totalBlades = blades[index].Count;
            bladeScript.UpdateBladePosition(); // 각도를 다시 설정
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
            // 수리검이 하나일 경우, 바로 위로 발사
            GameObject shuriken = GameManager.instance.poolManager.Get("Shuriken");
            shuriken.transform.position = GameManager.instance.player.transform.position;
            shuriken.transform.rotation = Quaternion.Euler(0, 0, 90);

            shuriken.GetComponent<Shuriken>().SetDirection(Vector2.up);
        }
        else
        {
            // 각도를 나누어서 여러 개 발사
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
