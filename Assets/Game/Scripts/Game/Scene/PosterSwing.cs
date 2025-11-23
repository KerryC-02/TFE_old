using UnityEngine;

public class PosterSwing : MonoBehaviour
{
    // 最小和最大旋转角度
    private float minAngle = 0.0f;
    private float maxAngle = 15.0f;
    // 当前目标角度
    private float targetAngle = 0.0f;
    // 旋转速度
    public float rotationSpeed = 5.0f;

    // 用于平滑旋转的角度
    private float currentAngle = 0.0f;

    void Start()
    {
        // 初始化目标角度为当前角度
        targetAngle = transform.eulerAngles.x;
        // 初始化当前角度
        currentAngle = transform.eulerAngles.x;
    }

    void Update()
    {
        // 当当前角度接近目标角度时，选择一个新的随机目标角度
        if (Mathf.Abs(currentAngle - targetAngle) < 0.5f)
        {
            targetAngle = Random.Range(minAngle, maxAngle);
        }

        // 平滑地旋转到目标角度
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(currentAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
