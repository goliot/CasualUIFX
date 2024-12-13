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

        // �̵� ������ ���� ���� ����
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
    }

    private Vector3 GetMouseWorldPos()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zDistance; // ��ü�� z��ǥ�� �����ϰ� ����
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
