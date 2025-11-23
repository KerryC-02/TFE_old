using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public AnimationClipData clip;
    public PlayerActionPerformDependency dependency;

    private float _currentAnimEndTime;

    void Start()
    {
        _currentAnimEndTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
            TryHit();
    }

    public void TryHit()
    {
        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        PlayerBehaviour.instance.animator.SetTrigger("hit");
        PlayerBehaviour.instance.drawGun.UndrawGunInstantly();
        _currentAnimEndTime = clip.GetEndTime();
    }

    public bool isHiting
    {
        get
        {
            return Time.time < _currentAnimEndTime;
        }
    }
}
