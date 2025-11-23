using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public EnemyAnimationClipData clip;
    public EnemyActionPerformDependency dependency;

    private EnemyBehaviour _host;

    public MonoBehaviour[] toDisables;
    public Collider col;

    public bool died { get; private set; }
    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
    }

    public void Die()
    {
        if (died)
            return;

        _host.animator.SetTrigger("die");

        foreach (var m in toDisables)
        {
            m.enabled = false;
        }
        col.enabled = false;
    }
}