using com;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public EnemyAnimationClipData clip;
    public EnemyActionPerformDependency dependency;

    private EnemyBehaviour _host;
    private float _currentAnimEndTime;

    public CharacterViewBehaviour.SpawnInfo bloodVfx;

    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
    }
    void Start()
    {
        _currentAnimEndTime = 0;
    }

    public void TryHit()
    {
        if (_host.death.died)
            return;

        if (!_host.AssertCondition(dependency))
            return;

        _host.animator.SetTrigger("hit");
        _currentAnimEndTime = clip.GetEndTime();

        TriggerSpawnBlood();

    }
    public void TriggerSpawnBlood(float externalDestroyTime = 5)
    {
        var pos = transform.position;
        if (_host.move.isFacingRight)
            pos += bloodVfx.offset;
        else
            pos += new Vector3(-bloodVfx.offset.x, bloodVfx.offset.y, bloodVfx.offset.z);

        var s = Instantiate(bloodVfx.prefab, pos, Quaternion.identity);
        if (!_host.move.isFacingRight)
            s.transform.localScale = new Vector3(-s.transform.localScale.x, s.transform.localScale.y, s.transform.localScale.z);

        if (externalDestroyTime > 0)
            Destroy(s.gameObject, externalDestroyTime);
    }

    public bool isHiting
    {
        get
        {
            return Time.time < _currentAnimEndTime;
        }
    }
}