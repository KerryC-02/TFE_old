using UnityEngine;
using System.Collections;

public class LightningFlash : MonoBehaviour
{
    public Light directionalLight;  // 引用定向光
    public float minIntensity = 0.2f;  // 闪电最小亮度
    public float maxIntensity = 8.0f;  // 闪电最大亮度
    public float flashDuration = 0.1f;  // 闪光持续时间
    public float timeBetweenFlashes = 2.0f;  // 两次闪电之间的时间间隔

    private bool isFlashing = false;  // 是否正在闪电

    void Update()
    {
        if (!isFlashing && Time.time >= nextFlashTime)
        {
            StartCoroutine(FlashLightning());
        }
    }

    private float nextFlashTime = 0f;

    IEnumerator FlashLightning()
    {
        isFlashing = true;
        float initialIntensity = directionalLight.intensity;  // 保存初始强度
        float flashTime = 0f;

        while (flashTime < flashDuration)
        {
            directionalLight.intensity = Random.Range(minIntensity, maxIntensity);  // 随机亮度模拟闪电
            flashTime += Time.deltaTime;
            yield return null;
        }

        directionalLight.intensity = initialIntensity;  // 恢复原始亮度
        nextFlashTime = Time.time + timeBetweenFlashes + Random.Range(-2f, 2f);  // 随机时间间隔
        isFlashing = false;
    }
}
