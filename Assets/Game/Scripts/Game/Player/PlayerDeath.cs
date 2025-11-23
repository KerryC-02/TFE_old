using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public AnimationClipData clip;
    public PlayerActionPerformDependency dependency;

    public MonoBehaviour[] toDisables;
    public Collider col;

    public bool died { get; private set; }

    public void Die()
    {
        if (died)
            return;

        PlayerBehaviour.instance.animator.SetTrigger("die");

        foreach (var m in toDisables)
        {
            m.enabled = false;
        }
        //col.enabled = false;
    }
}