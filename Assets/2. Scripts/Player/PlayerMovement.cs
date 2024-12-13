using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public bool isMoving = true; // 움직임 제어를 위한 플래그

    private void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    public void ChangeMoveState(bool value)
    {
        isMoving = value;
    }
}
