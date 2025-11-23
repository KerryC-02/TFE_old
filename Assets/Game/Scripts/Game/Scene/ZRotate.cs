using UnityEngine;

public class ZRotate : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // 旋转速度，单位为度/秒

    void Update()
    {
        // 每帧都在Z轴上顺时针旋转，考虑到Unity的旋转方向，需要使用负号
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}

