using UnityEngine;

public class DestroyAfterAnimEnd : MonoBehaviour
{
    public AnimationClipData clipData;

    private void Start()
    {
        Destroy(gameObject, clipData.clip.length * clipData.speedRatio);
    }
}