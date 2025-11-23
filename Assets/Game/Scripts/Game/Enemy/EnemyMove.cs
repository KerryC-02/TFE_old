using UnityEngine;
using static EnemyMove;

public class EnemyMove : MonoBehaviour
{
    public EnemyActionPerformDependency dependency;

    [SerializeField] private SpriteRenderer _sr;
    public enum EnemyMoveStyle
    {
        Stand = 0,
        Patrol = 1,
        Static = 2,
    }

    public EnemyMoveStyle moveStyle;

    [SerializeField] private Transform _patrolLeft;
    [SerializeField] private Transform _patrolRight;
    [SerializeField] private float _patrolStandDuration;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _crouchMoveSpeed;

    public bool startStandFacingRight;

    private enum PatrolState
    {
        GoLeft,
        GoRight,
        StandAtLeft,
        StandAtRight,
    }

    private PatrolState _patrolState;
    private float _patrolStateEndTime;
    private Vector3 _startPos;

    private EnemyBehaviour _host;
    public bool isMoving { get; private set; }

    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
    }
    void Start()
    {
        _startPos = transform.position;

        switch (moveStyle)
        {
            case EnemyMoveStyle.Stand:
                _host.animator.SetBool("walk", false);
                _sr.flipX = !startStandFacingRight;
                _host.attack.cvb.SyncView();
                break;
            case EnemyMoveStyle.Patrol:
                _patrolState = PatrolState.GoLeft;
                _patrolStateEndTime = 0;
                _host.animator.SetBool("walk", true);
                _patrolLeft.SetParent(transform.parent);
                _patrolRight.SetParent(transform.parent);
                break;
            case EnemyMoveStyle.Static:
                //_host.animator.SetBool("walk", false);
                _sr.flipX = !startStandFacingRight;
                _host.attack.cvb.SyncView();
                break;
        }
    }

    public void OnAlertLevelChanged(EnemyAlert.AlertLevel newLevel)
    {
        switch (newLevel)
        {
            case EnemyAlert.AlertLevel.None:
                //recalculate _patrolState 
                switch (moveStyle)
                {
                    case EnemyMoveStyle.Stand:
                        _host.animator.SetBool("walk", false);
                        break;
                    case EnemyMoveStyle.Patrol:
                        switch (_patrolState)
                        {
                            case PatrolState.GoLeft:
                                if (transform.position.x < _patrolLeft.position.x)
                                {
                                    _patrolState = PatrolState.StandAtLeft;
                                    _patrolStateEndTime = Time.time + _patrolStandDuration;
                                    _host.animator.SetBool("walk", false);
                                }
                                else
                                {
                                    _host.animator.SetBool("walk", true);
                                    MoveLeft();
                                }
                                break;
                            case PatrolState.GoRight:
                                if (transform.position.x > _patrolRight.position.x)
                                {
                                    _patrolState = PatrolState.StandAtRight;
                                    _patrolStateEndTime = Time.time + _patrolStandDuration;
                                    _host.animator.SetBool("walk", false);
                                }
                                else
                                {
                                    _host.animator.SetBool("walk", true);
                                    MoveRight();
                                }
                                break;
                            case PatrolState.StandAtLeft:
                                _patrolStateEndTime = Time.time;
                                break;
                            case PatrolState.StandAtRight:
                                _patrolStateEndTime = Time.time;
                                break;
                        }
                        break;
                    case EnemyMoveStyle.Static:
                        //_host.animator.SetBool("walk", false);
                        break;
                }
                break;
            case EnemyAlert.AlertLevel.Warning:
                switch (moveStyle)
                {
                    case EnemyMoveStyle.Stand:

                        break;
                    case EnemyMoveStyle.Patrol:

                        break;
                    case EnemyMoveStyle.Static:

                        break;
                }
                var warningTargetPosition = _host.alert.target.position;
                if (warningTargetPosition.x > transform.position.x)
                {
                    FlipRight();
                }
                else
                {
                    FlipLeft();
                }
                break;
            case EnemyAlert.AlertLevel.Aggressive:
                //Turn to target 
                var targetPosition = _host.alert.target.position;
                if (targetPosition.x > transform.position.x)
                {
                    FlipRight();
                }
                else
                {
                    FlipLeft();
                }
                break;
        }
    }

    void Update()
    {
        switch (_host.alert.alertLevel)
        {
            case EnemyAlert.AlertLevel.None:
                TryNoAlertMove();
                break;
            case EnemyAlert.AlertLevel.Warning:
                TryWarningMove();
                break;
            case EnemyAlert.AlertLevel.Aggressive:
                TryAggressiveMove();
                break;
        }
    }

    /// <summary>
    /// 当敌人没有警戒状态时，可以进行TryNoAlertMove
    /// 当已经偏离出生点时，会先回到出生点
    /// 否则会继续正常的行动
    /// </summary>
    public void TryNoAlertMove()
    {
        if (!_host.AssertCondition(dependency))
            return;

        switch (moveStyle)
        {
            case EnemyMoveStyle.Stand:
                Stand();
                break;
            case EnemyMoveStyle.Patrol:
                Patrol();
                break;
            case EnemyMoveStyle.Static:
                Stand();
                break;
        }
    }

    /// <summary>
    /// 当敌人警觉状态时，可以进行TryWarningMove
    /// 会移动到指定地点，并停留一段时间
    /// 之后会回到TryNoAlertMove的状态
    /// 
    /// TODO
    /// create an object that emit warning sign to enemies nearby
    /// </summary>
    public void TryWarningMove()
    {
        if (!_host.AssertCondition(dependency))
            return;

        switch (moveStyle)
        {
            case EnemyMoveStyle.Stand:
                Stand();
                break;
            case EnemyMoveStyle.Patrol:
                Patrol();
                break;
            case EnemyMoveStyle.Static:
                Stand();
                break;
        }
    }

    public float displayLastDistance;

    /// <summary>
    /// 当敌人处于警戒状态时，可以进行TryAggressiveMove
    /// 首先一定会面朝目标
    /// 如果目标在射程外，则向目标移动
    /// 如果目标在射程内，则向目标攻击
    /// 
    /// </summary>
    public void TryAggressiveMove()
    {
        if (!_host.AssertCondition(dependency))
            return;

        var targetPosition = _host.alert.target.position;
        var targetIsInRight = targetPosition.x > transform.position.x;
        if (targetIsInRight)
            FlipRight();
        else
            FlipLeft();


        switch (moveStyle)
        {
            case EnemyMoveStyle.Stand:
                Stand();
                break;
            case EnemyMoveStyle.Patrol:
                var targetInRange = _host.attack.isTargetInRange;
                displayLastDistance = Mathf.Abs(targetPosition.x - transform.position.x);
                if (targetInRange)
                {
                    //player is in enemy's attack range
                    _host.animator.SetBool("walk", false);
                }
                else
                {
                    //player is out of enemy's attack range
                    _host.animator.SetBool("walk", true);
                    if (targetIsInRight)
                        MoveRight();
                    else
                        MoveLeft();
                }
                break;
            case EnemyMoveStyle.Static:
                Stand();
                break;
        }


    }

    private void Stand()
    {
        //Do nothing
    }

    private void Patrol()
    {
        switch (_patrolState)
        {
            case PatrolState.GoLeft:
                MoveLeft();
                if (transform.position.x < _patrolLeft.position.x)
                {
                    _patrolState = PatrolState.StandAtLeft;
                    _patrolStateEndTime = Time.time + _patrolStandDuration;
                    _host.animator.SetBool("walk", false);
                }
                break;
            case PatrolState.GoRight:
                MoveRight();
                if (transform.position.x > _patrolRight.position.x)
                {
                    _patrolState = PatrolState.StandAtRight;
                    _patrolStateEndTime = Time.time + _patrolStandDuration;
                    _host.animator.SetBool("walk", false);
                }
                break;
            case PatrolState.StandAtLeft:
                if (Time.time > _patrolStateEndTime)
                {
                    _patrolState = PatrolState.GoRight;
                    _host.animator.SetBool("walk", true);
                }
                else
                    Stand();
                break;
            case PatrolState.StandAtRight:
                if (Time.time > _patrolStateEndTime)
                {
                    _patrolState = PatrolState.GoLeft;
                    _host.animator.SetBool("walk", true);
                }
                else
                    Stand();
                break;
        }
    }

    private void MoveLeft()
    {
        Move(-_walkSpeed);
        FlipLeft();
    }

    private void MoveRight()
    {
        Move(_walkSpeed);
        FlipRight();
    }

    public bool isFacingRight
    {
        get { return !_sr.flipX; }
    }

    public void FlipRight()
    {
        _sr.flipX = false;
    }

    public void FlipLeft()
    {
        _sr.flipX = true;
    }

    private void Move(float speed)
    {
        transform.position += Time.deltaTime * new Vector3(speed, 0, 0);
    }
}