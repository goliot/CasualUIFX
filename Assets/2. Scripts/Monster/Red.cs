using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : Monster
{
    private float curTime;

    public override void Update()
    {
        base.Update();

        curTime += Time.deltaTime;
        if(curTime > atkSpeed)
        {
            Shoot();
            curTime = 0;
        }
    }

    private void Shoot()
    {
        GameObject bullet = GameManager.instance.poolManager.Get("RedBullet");
        bullet.transform.position = transform.position;
        bullet.GetComponent<RedBullet>().damage = damage;
    }
}
