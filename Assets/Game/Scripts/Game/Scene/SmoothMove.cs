using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public float moveRange = 0.5f;  // 移动范围
    public float moveSpeed = 1.0f;  // 控制移动速度

    private float startPositionX;
    private float targetPositionX;
    private float moveDirection = 1.0f;  // 移动方向

    void Start()
    {
        startPositionX = transform.position.x;  // 记录初始X位置
    }

    void Update()
    {
        // 计算目标位置
        targetPositionX = startPositionX + moveRange * Mathf.Sin(Time.time * moveSpeed);

        // 使用SmoothStep函数实现缓进缓出的移动效果
        float newX = Mathf.SmoothStep(transform.position.x, targetPositionX, Time.deltaTime * moveSpeed);

        // 更新物体的位置
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
