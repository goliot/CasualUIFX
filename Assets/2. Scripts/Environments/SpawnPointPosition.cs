using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPosition : MonoBehaviour
{
    void Update()
    {
        transform.position = GameManager.instance.cameraController.gameObject.transform.position;
    }
}
