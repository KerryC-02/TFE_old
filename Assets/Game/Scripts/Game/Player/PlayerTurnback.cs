using UnityEngine;

public class PlayerTurnback : MonoBehaviour
{
    public KeyCode toggleTurnKey = KeyCode.T;

    public bool turnedBack { get; private set; }

    public AnimationClipData enter;
    public AnimationClipData exit;
    public PlayerActionPerformDependency dependency;
    private float _currentAnimEndTime;

    void Start()
    {
        _currentAnimEndTime = 0;
        turnedBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //  if (Input.GetKeyDown(toggleTurnKey))
        //    TryToggleTurn();
    }

    public void TryTurnBack()
    {
        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        turnedBack = true;
        PlayerBehaviour.instance.animator.SetTrigger("turn");
        PlayerBehaviour.instance.animator.ResetTrigger("turn reversed");
        _currentAnimEndTime = exit.GetEndTime();
    }

    public void TryReverseTurnBack()
    {
        if (!turnedBack)
        {
            return;
        }

        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        turnedBack = false;
        PlayerBehaviour.instance.animator.SetTrigger("turn reversed");
        PlayerBehaviour.instance.animator.ResetTrigger("turn");
        _currentAnimEndTime = enter.GetEndTime();
    }

    void TryToggleTurn()
    {
        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        if (turnedBack)
        {
            turnedBack = false;
            PlayerBehaviour.instance.animator.SetTrigger("turn reversed");
            _currentAnimEndTime = enter.GetEndTime();
        }
        else
        {
            turnedBack = true;
            PlayerBehaviour.instance.animator.SetTrigger("turn");
            _currentAnimEndTime = exit.GetEndTime();
        }
    }

    public bool isTurning
    {
        get
        {
            return Time.time < _currentAnimEndTime;
        }
    }
}
