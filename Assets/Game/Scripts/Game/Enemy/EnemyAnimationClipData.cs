using UnityEngine;

[System.Serializable]
public struct EnemyAnimationClipData
{
    public AnimationClip clip;
    public float speedRatio;

    public float GetEndTime()
    {
        return Time.time + clip.length / speedRatio;
    }
}

[System.Serializable]
public struct EnemyActionPerformDependency
{
    [Header("check the conditions which is not allowed to perform this action")]
    public bool notAggressive;
    public bool notWarning;
    public bool notShooting;
    public bool notWalking;
    public bool notHiting;
}