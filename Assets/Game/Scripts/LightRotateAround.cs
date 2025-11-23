using UnityEngine;

public class LightRotateAround : MonoBehaviour
{
    public Transform rotationCenter; // 旋转中心点
    public float rotationSpeed = 50f; // 旋转速度，可以在Unity编辑器中调整

    void Update()
    {
        if (rotationCenter != null)
        {
            // 围绕rotationCenter旋转，每秒rotationSpeed度，沿着Y轴
            transform.RotateAround(rotationCenter.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
