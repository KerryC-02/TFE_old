using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    private EnemyBehaviour _host;

    public float sightRange;
    public float aggressivePresistDuration;
    public float warningPresistDuration;
    public Transform target { get; private set; }

    public enum AlertLevel
    {
        None,
        Warning,
        Aggressive,
    }

    private AlertLevel _alertLevel;
    public AlertLevel alertLevel
    {
        get
        {
            return _alertLevel;
        }
        set
        {
            if (_alertLevel != value)
            {
                _alertLevel = value;
                _host.OnAlertLevelChanged(value);
            }
        }
    }

    private float _exitAggressiveTime;

    private float _exitWarningTime;

    private Vector3 _warnCheckOrigin;

    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
    }
    void Start()
    {

    }

    void Update()
    {
        CheckSight();
        CheckStateChangeByTime();
    }

    void CheckStateChangeByTime()
    {
        switch (alertLevel)
        {
            case AlertLevel.None:
                break;
            case AlertLevel.Warning:
                if (Time.time > _exitWarningTime)
                    alertLevel = AlertLevel.None;
                break;
            case AlertLevel.Aggressive:
                if (Time.time > _exitAggressiveTime)
                    alertLevel = AlertLevel.None;
                break;
        }
    }

    /// <summary>
    /// 敌人收到一个警告来源的位置信息orgin，并且如果当前alertLevel是None，则回去查看，alertLevel进入Warning状态
    /// </summary>
    public void OnNotified(Vector3 orgin)
    {
        if (alertLevel != AlertLevel.None)
            return;
         if(_host.death.died)
            return;
        _exitWarningTime = Time.time + warningPresistDuration;
        _warnCheckOrigin = orgin;

        alertLevel = AlertLevel.Warning;
    }

    /// <summary>
    /// 敌人被攻击后，会收到一个伤害来源的位置信息，alertLevel进入Warning状态
    /// </summary>
    /// <param name="orgin"></param>
    public void OnAttacked(Vector3 orgin)
    {
        if (alertLevel == AlertLevel.Aggressive)
            return;

        _exitWarningTime = Time.time + warningPresistDuration;
        _warnCheckOrigin = orgin;

        alertLevel = AlertLevel.Warning;
    }

    /// <summary>
    /// 敌人会自主查看视野范围内是否有敌对目标，如果发现，alertLevel进入Aggressive状态
    /// </summary>
    public void CheckSight()
    {
        var playerPos = PlayerBehaviour.instance.transform.position;
        var deltaX = transform.position.x - playerPos.x;
        var deltaY = transform.position.y - playerPos.y;

        //TODO y distance check
        //TODO player hide
        //TODO sight blocked by obstacle
        if (Mathf.Abs(deltaX) < sightRange)
        {
            if (_host.move.isFacingRight && deltaX < 0)
            {
                OnPlayerInSight();
            }
            else if (!_host.move.isFacingRight && deltaX > 0)
            {
                OnPlayerInSight();
            }
        }
    }

    void OnPlayerInSight()
    {
        _exitAggressiveTime = Time.time + aggressivePresistDuration;
        target = PlayerBehaviour.instance.transform;
        alertLevel = AlertLevel.Aggressive;
    }
}