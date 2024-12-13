using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private float moveAmount;
    [SerializeField]
    private float maxDistance;

    public bool isStopped = false;
    private void Start()
    {
        isStopped = false;
    }

    private void Update()
    {
        if (isStopped) return;

        if (Vector2.Distance(sprites[endIndex].position, player.position) > maxDistance)
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            sprites[endIndex].localPosition = backSpritePos + Vector3.up * moveAmount;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = startIndexSave - 1 == -1 ? sprites.Length - 1 : --startIndexSave;
        }
    }
}
