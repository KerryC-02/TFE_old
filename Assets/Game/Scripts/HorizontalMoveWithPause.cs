using UnityEngine;
using System.Collections;

public class HorizontalMoveWithPause : MonoBehaviour
{
    public float speed = 2.0f;        // 移动速度
    public float moveDistance = 5.0f; // 左右移动的最大距离
    public float pauseTime = 2.0f;    // 达到边缘时的停顿时间

    private float originalX; // 初始X位置
    private bool movingRight = true; // 判断是否向右移动
    private bool isPaused = false;   // 是否正在暂停中

    void Start()
    {
        originalX = transform.position.x;
    }

    void Update()
    {
        if (isPaused) return;

        float targetX = movingRight ? originalX + moveDistance : originalX - moveDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), speed * Time.deltaTime);

        // 检查是否到达目标位置
        if (Mathf.Approximately(transform.position.x, targetX))
        {
            StartCoroutine(PauseAtEnd());
        }
    }

    IEnumerator PauseAtEnd()
    {
        isPaused = true;

        // 翻转Sprite
        if (movingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        yield return new WaitForSeconds(pauseTime);

        // 翻转Sprite回初始方向
        if (!movingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        movingRight = !movingRight; // 改变移动方向
        isPaused = false;
    }
}
