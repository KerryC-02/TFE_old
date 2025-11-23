using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Animator animator { get; private set; }
    public EnemyAlert alert { get; private set; }
    public EnemyAttack attack { get; private set; }
    public EnemyDeath death { get; private set; }
    public EnemyHealth health { get; private set; }
    public EnemyMove move { get; private set; }
    public EnemyHit hit { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        alert = GetComponent<EnemyAlert>();
        attack = GetComponent<EnemyAttack>();
        move = GetComponent<EnemyMove>();
        death = GetComponent<EnemyDeath>();
        health = GetComponent<EnemyHealth>();
        hit = GetComponent<EnemyHit>();
    }

    public bool AssertCondition(EnemyActionPerformDependency dependency)
    {
        if (dependency.notAggressive && alert.alertLevel == EnemyAlert.AlertLevel.Aggressive)
            return false;
        if (dependency.notWarning && alert.alertLevel == EnemyAlert.AlertLevel.Warning)
            return false;
        if (dependency.notShooting && attack.isAttacking)
            return false;
        if (dependency.notWalking && move.isMoving)
            return false;
        if (dependency.notHiting && hit.isHiting)
            return false;

        return true;
    }

    public void OnAlertLevelChanged(EnemyAlert.AlertLevel newLevel)
    {
        Debug.Log("OnAlertLevelChanged " + newLevel);
        move.OnAlertLevelChanged(newLevel);

        animator.SetBool("has target", newLevel == EnemyAlert.AlertLevel.Aggressive);
    }
}