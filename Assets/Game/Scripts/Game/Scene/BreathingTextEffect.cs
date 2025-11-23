using UnityEngine;
using TMPro; // 引入TextMeshPro命名空间

public class BreathingTextEffect : MonoBehaviour
{
    public TMP_Text textMesh; // 要控制的TextMeshPro组件
    public float minAlpha = 0.2f; // 最小透明度
    public float maxAlpha = 1.0f; // 最大透明度
    public float pulseSpeed = 2.0f; // 呼吸速度

    private void Update()
    {
        // 计算当前透明度，使用正弦函数来实现呼吸效果
        float alpha = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f) * (maxAlpha - minAlpha) + minAlpha;

        // 获取当前的颜色
        Color currentColor = textMesh.color;

        // 更新颜色的alpha值
        currentColor.a = alpha;

        // 将更新后的颜色赋回给TextMeshPro组件
        textMesh.color = currentColor;
    }
}
