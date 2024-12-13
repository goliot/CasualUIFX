using UnityEngine;

public class DragMove : MonoBehaviour
{
    [SerializeField]
    private float minX = -1.7f;
    [SerializeField]
    private float maxX = 1.7f;

    private Vector3 offset;
    private float zDistance;

    private void OnMouseDown()
    {
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        Vector3 mouseWorldPos = GetMouseWorldPos();
        Vector3 newPosition = new Vector3(mouseWorldPos.x + offset.x, transform.position.y, transform.position.z);

        // 이동 범위를 벽에 맞춰 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
    }

    private Vector3 GetMouseWorldPos()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zDistance; // 객체의 z좌표와 동일하게 유지
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
