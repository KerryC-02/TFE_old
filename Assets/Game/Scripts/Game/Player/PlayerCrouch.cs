using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public KeyCode crouchKey = KeyCode.Z;

    bool _lastCrouchIsPressed;
    bool _currentCrouchIsPressed;
    public bool isCrouching { get; private set; }

    float _nextCanChangeStateTime;

    public AnimationClipData enter;
    public AnimationClipData exit;
    public PlayerActionPerformDependency dependency;

    void Start()
    {
        _nextCanChangeStateTime = 0;
    }

    public bool isPerformingAction
    {
        get { return Time.time < _nextCanChangeStateTime; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(crouchKey))
            _currentCrouchIsPressed = true;
        if (Input.GetKeyUp(crouchKey))
            _currentCrouchIsPressed = false;

        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        if (_currentCrouchIsPressed != _lastCrouchIsPressed)
        {
            _lastCrouchIsPressed = _currentCrouchIsPressed;

            //crouch状态有了变化
            if (!isCrouching && _currentCrouchIsPressed)
            {
                //进入crouch
                isCrouching = true;
                _nextCanChangeStateTime = enter.GetEndTime();
                PlayerBehaviour.instance.animator.SetBool("crouch", true);
            }
            else if (isCrouching && !_currentCrouchIsPressed)
            {
                //结束crouch
                isCrouching = false;
                _nextCanChangeStateTime = exit.GetEndTime();
                PlayerBehaviour.instance.animator.SetBool("crouch", false);
            }
        }
    }
}