using Assets.Game.Scripts.Game.Scene;
using UnityEngine;

public class PlayerDrawGun : MonoBehaviour
{
    bool _lastIsPressed;
    bool _currentIsPressed;
    public bool hasGunDrawn { get; private set; }
    public bool disableDrawGun;

    float _nextCanChangeStateTime;

    public AnimationClipData enter;
    public AnimationClipData exit;
    public PlayerActionPerformDependency dependency;

    void Start()
    {
        _nextCanChangeStateTime = 0;
    }

    public void UndrawGunInstantly()
    {
        hasGunDrawn = false;
        PlayerBehaviour.instance.animator.SetBool("has gun", false);
    }

    public bool isPerformingAction
    {
        get { return Time.time < _nextCanChangeStateTime; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            _currentIsPressed = true;
        if (Input.GetMouseButtonUp(1))
            _currentIsPressed = false;

        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        if (disableDrawGun && hasGunDrawn)
        {
            UndrawGun();
        }

        if (_currentIsPressed != _lastIsPressed)
        {
            _lastIsPressed = _currentIsPressed;

            if (_currentIsPressed && !hasGunDrawn && !disableDrawGun)
            {
                //½øÈë
                hasGunDrawn = true;
                _nextCanChangeStateTime = enter.GetEndTime();
                PlayerBehaviour.instance.animator.ResetTrigger("undraw gun");
                PlayerBehaviour.instance.animator.SetTrigger("draw gun");
                PlayerBehaviour.instance.animator.SetBool("has gun", true);
            }
            else if (!_currentIsPressed && hasGunDrawn)
            {
                UndrawGun();
            }
        }
    }

    void UndrawGun()
    {
        //½áÊø
        hasGunDrawn = false;
        _nextCanChangeStateTime = exit.GetEndTime();
        PlayerBehaviour.instance.animator.ResetTrigger("draw gun");
        PlayerBehaviour.instance.animator.SetTrigger("undraw gun");
        PlayerBehaviour.instance.animator.SetBool("has gun", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");
        var smt = other.GetComponent<SceneModiferTrigger>();
        if (!(smt is null))
        {
            if (smt.modify_disableDrawGun)
            {
                disableDrawGun = smt.modify_disableDrawGun_value;
            }
        }
    }
}