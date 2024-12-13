using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    private float curTime;

    public override void Update()
    {
        base.Update();

        curTime += Time.deltaTime;
        if(curTime > atkSpeed)
        {
            BossSkill();
            curTime = 0;
        }
    }

    private void BossSkill()
    {
        StartCoroutine(FireRain());
    }

    IEnumerator FireRain()
    {
        int cnt = 0;
        while(cnt < 10)
        {
            GameObject bullet = GameManager.instance.spawner.SpawnFireBallRain();
            bullet.GetComponent<RedBullet>().damage = damage;
            yield return new WaitForSeconds(0.2f);
            cnt++;
        }
    }
}
