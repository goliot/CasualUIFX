using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public bool isMoving = true; // ������ ��� ���� �÷���

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
