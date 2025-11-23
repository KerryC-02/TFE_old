using UnityEngine;

public class LightMoveAndRotate : MonoBehaviour
{
    public float moveSpeed = 3.0f;  // 控制移动速度
    public float rotateSpeed = 50.0f;  // 控制旋转速度
    public float moveRange = 5.0f;  // 控制移动范围，从中心点向两侧移动的最大距离

    private Vector3 startPosition;  // 初始位置

    void Start()
    {
        // 保存初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 计算当前位置与时间的正弦波形，使物体来回移动
        float xPosition = Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = new Vector3(startPosition.x + xPosition, startPosition.y, startPosition.z);

        // 沿Y轴旋转
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }
}

