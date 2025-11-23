using UnityEngine;

public class HorizontalMove : MonoBehaviour
{
    public float speed = 2.0f;        // 移动速度
    public float moveDistance = 5.0f; // 左右移动的最大距离
    public float pauseTime = 2.0f;    // 达到边缘时的停顿时间

    private float originalX; // 初始X位置
    private float nextMoveTime; // 下一次移动的时间
    private bool movingRight = true; // 判断是否向右移动

    void Start()
    {
        originalX = transform.position.x;
        nextMoveTime = Time.time; // 初始化下一次移动时间
    }

    void Update()
    {
        if (Time.time < nextMoveTime) return; // 如果未达到下一次移动时间，直接返回

        float targetX = movingRight ? originalX + moveDistance : originalX - moveDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), speed * Time.deltaTime);

        // 检查是否到达目标位置
        if (Mathf.Approximately(transform.position.x, targetX))
        {
            movingRight = !movingRight; // 改变移动方向
            nextMoveTime = Time.time + pauseTime; // 设置下一次移动时间

            // 翻转Sprite
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
