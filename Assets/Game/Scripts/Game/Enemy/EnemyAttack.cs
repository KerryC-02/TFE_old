using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class EnemyAttack : MonoBehaviour
{
    public EnemyAnimationClipData clip;
    public EnemyActionPerformDependency dependency;

    private EnemyBehaviour _host;
    private float _currentAnimEndTime;

    public float attackIntervalExtra;
    public float attackRange;

    public CharacterViewBehaviour cvb;
    public float shootRange = 3.6f;
    public LayerMask playerLayerMask;

    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
        if (cvb == null)
            cvb = GetComponent<CharacterViewBehaviour>();
    }
    void Start()
    {
        _currentAnimEndTime = 0;

    }

    void Update()
    {
        if (isAttacking)
            return;
        if (_host.alert.alertLevel != EnemyAlert.AlertLevel.Aggressive)
            return;
        if (_host.alert.target == null)
            return;
        if (_host.move.isFacingRight && _host.alert.target.position.x <= transform.position.x)
        {
            //Debug.Log("not facing target");
            return;
        }
        if (!_host.move.isFacingRight && _host.alert.target.position.x >= transform.position.x)
        {
            //Debug.Log("not facing target");
            return;
        }
        //Debug.Log("TryAttack");
        TryAttack();
    }

    public void TryAttack()
    {
        if (!_host.AssertCondition(dependency))
            return;

        if (!isTargetInRange)
            return;

        _host.animator.SetTrigger("attack");
        _host.animator.SetBool("walk", false);

        bool isLongInterval = ((shootLoopCount + 1) % 2 == 0);
        // wait a little longer after 3 shots

        _currentAnimEndTime = clip.GetEndTime();
        if (isLongInterval)
            _currentAnimEndTime += attackIntervalExtra;

        //Debug.Log("isLongInterval " + isLongInterval);
        shootLoopCount++;
    }

    private int shootLoopCount = 0;

    public void OnAlertLevelChanged(EnemyAlert.AlertLevel newLevel)
    {
        //Debug.Log("OnAlertLevelChanged ");
        shootLoopCount = 0;
    }

    public void OnFire()
    {
        //TODO make bullet
        Debug.Log("enemy OnFire");
        cvb.OnFire();
        CheckShoot();
    }

    void CheckShoot()
    {
        var rayStart = cvb.viewPos;
        Vector3 direction = new Vector2(_host.move.isFacingRight ? 1 : -1, 0);
        //Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask,
        RaycastHit ray;
        if (Physics.Raycast(rayStart, direction, out ray, shootRange, playerLayerMask))
        {
            Debug.Log(ray.collider);
            if (ray.collider != null)
            {
                Debug.Log("ene shoot hit!");
                var p = ray.collider.GetComponent<PlayerBehaviour>();
                if (p != null)
                {
                    p.health.TakeDamage(1);
                }
            }
        }
    }

    public bool isTargetInRange
    {
        get
        {
            var result = false;
            if (_host.alert.target == null)
            {
                return result;
            }

            var distVector = _host.alert.target.position - transform.position;
            var distX = Mathf.Abs(distVector.x);

            if (attackRange >= distX)
            {
                //player is in enemy's attack range
                result = true;
            }
            else
            {
                //player is out of enemy's attack range
                result = false;
            }
            return result;
        }
    }
    public bool isAttacking
    {
        get
        {
            return Time.time < _currentAnimEndTime;
        }
    }
}