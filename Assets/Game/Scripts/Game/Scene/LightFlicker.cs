using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;    // 用于闪烁的光源
    public float flickerSpeed = 0.5f; // 闪烁速度，单位为秒
    public float maxIntensity = 1.0f; // 光源的最大亮度

    private bool isLightOn = true; // 控制光源开/关状态的标志
    private float nextFlickerTime; // 下一次切换光源状态的时间点

    void Update()
    {
        // 检查是否到了需要切换光源状态的时间
        if (Time.time >= nextFlickerTime)
        {
            // 更新下一次切换光源状态的时间点
            nextFlickerTime = Time.time + flickerSpeed;

            // 切换光源的开/关状态
            isLightOn = !isLightOn;
            flickerLight.intensity = isLightOn ? maxIntensity : 0;
        }
    }
}
