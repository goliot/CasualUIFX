using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void OnExplosionEnd()
    {
        GameManager.instance.poolManager.Release(gameObject);
    }
}
