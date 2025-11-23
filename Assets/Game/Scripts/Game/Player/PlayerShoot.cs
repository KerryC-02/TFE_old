using System;
using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private float _currentAnimEndTime;
    public AnimationClipData clip;
    public PlayerActionPerformDependency dependency;
    [SerializeField] CharacterViewBehaviour _cvb;
    private PlayerBehaviour _host;
    public float shootRange = 3.6f;
    public LayerMask eneLayerMask;

    void Start()
    {
        _host = GetComponent<PlayerBehaviour>();

        _currentAnimEndTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryShoot();
    }

    void TryShoot()
    {
        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;
        if (!PlayerBehaviour.instance.drawGun.hasGunDrawn)
            return;

        PlayerBehaviour.instance.animator.SetTrigger("shoot");
        _currentAnimEndTime = clip.GetEndTime();
        //Debug.Log("TryShoot!");
        var rayStart = _cvb.viewPos;
        Vector3 direction = new Vector2(_host.move.isFacingRight ? 1 : -1, 0);
        //Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask,
        RaycastHit ray;
        if (Physics.Raycast(rayStart, direction, out ray, shootRange, eneLayerMask))
        {
            Debug.Log(ray.collider);
            if (ray.collider != null)
            {
                Debug.Log("shoot hit!");
                var eb = ray.collider.GetComponent<EnemyBehaviour>();
                if (eb != null)
                {
                    eb.health.TakeDamage(1);
                }

                var tb = ray.collider.GetComponent<TurretBehaviour>();
                if (tb != null)
                {
                    tb.TakeDamage(1);
                }
            }
        }
    }

    public bool isShooting
    {
        get
        {
            return Time.time < _currentAnimEndTime;
        }
    }

    public void OnFire()
    {
        _cvb.OnFire();
    }
}
