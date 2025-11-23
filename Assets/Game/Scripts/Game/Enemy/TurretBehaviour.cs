using TMPro;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private float _currentAnimEndTime;

    public float attackIntervalExtra;
    public float attackRange;
    public float attackInterval = 0.7f;
    [SerializeField] CharacterViewBehaviour _cvb;
    public float shootRange = 3.6f;
    public LayerMask playerLayerMask;
    private bool _lastPlayerInRange;
    public bool isFacingRight;

    public int health;
    public CharacterViewBehaviour.SpawnInfo dieVfx;
    public CharacterViewBehaviour.SpawnInfo hitVfx;
    private SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _currentAnimEndTime = 0;
        SyncFacing();
    }

    void SyncFacing()
    {
        _sr.flipX = isFacingRight ? false : true;
        _cvb.SyncView();
    }

    void Update()
    {
        if (isAttacking)
            return;

        //Debug.Log("TryAttack");
        TryAttack();
    }

    public void TryAttack()
    {
        var itir = isTargetInRange;
        if (!itir)
        {
            _lastPlayerInRange = false;
            return;
        }

        if (!_lastPlayerInRange)
            shootLoopCount = 0;
        _lastPlayerInRange = true;

        bool isLongInterval = ((shootLoopCount + 1) % 3 == 0);
        // wait a little longer after 3 shots

        _currentAnimEndTime = Time.time + attackInterval;
        if (isLongInterval)
            _currentAnimEndTime += attackIntervalExtra;

        shootLoopCount++;
        _cvb.OnFire();
        CheckShoot();
    }

    private int shootLoopCount = 0;

    void CheckShoot()
    {
        var rayStart = _cvb.viewPos;
        Vector3 direction = new Vector2(isFacingRight ? 1 : -1, 0);
        //Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask,
        RaycastHit ray;
        if (Physics.Raycast(rayStart, direction, out ray, shootRange, playerLayerMask))
        {
            Debug.Log(ray.collider);
            if (ray.collider != null)
            {
                Debug.Log("turret shoot hit!");
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
            //  shootLoopCount = 0;
            var distVector = PlayerBehaviour.instance.transform.position - transform.position;
            if (isFacingRight && distVector.x < 0)
                return false;
            if (!isFacingRight && distVector.x > 0)
                return false;

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

    public void TakeDamage(int v)
    {
        health -= v;
        if (health <= 0)
        {
            Die();
            return;
        }

        Hit();
    }


    void Hit()
    {
        var pos = transform.position;
        var s = Instantiate(hitVfx.prefab, pos, Quaternion.identity);
        Destroy(s.gameObject, 2);
    }

    void Die()
    {
        TriggerSpawnExplosion();
        Destroy(gameObject, 0.1f);
    }

    public void TriggerSpawnExplosion(float externalDestroyTime = 5)
    {
        var pos = transform.position;
        var s = Instantiate(dieVfx.prefab, pos, Quaternion.identity);
        if (externalDestroyTime > 0)
            Destroy(s.gameObject, externalDestroyTime);
    }
}