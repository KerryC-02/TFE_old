using Assets.Game.Scripts.Game.Scene;
using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour instance;

    public Animator animator { get; private set; }
    public PlayerCrouch crouch { get; private set; }
    public PlayerDrawGun drawGun { get; private set; }
    public PlayerMove move { get; private set; }
    public PlayerTurnback turnback { get; private set; }
    public PlayerShoot shoot { get; private set; }
    public PlayerHit hit { get; private set; }
    public PlayerHealth health { get; private set; }
    public PlayerDeath death { get; private set; }

    public CinemachineVirtualCamera cvc;

    private void Awake()
    {
        instance = this;

        animator = GetComponentInChildren<Animator>();
        crouch = GetComponent<PlayerCrouch>();
        drawGun = GetComponent<PlayerDrawGun>();
        move = GetComponent<PlayerMove>();
        turnback = GetComponent<PlayerTurnback>();
        shoot = GetComponent<PlayerShoot>();
        hit = GetComponent<PlayerHit>();
        health = GetComponent<PlayerHealth>();
        death = GetComponent<PlayerDeath>();

        cvc = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public bool AssertCondition(PlayerActionPerformDependency dependency)
    {
        if (dependency.notCrouching && crouch.isCrouching)
            return false;
        if (dependency.notPerformingCrouching && crouch.isPerformingAction)
            return false;

        if (dependency.noGunDrawn && drawGun.hasGunDrawn)
            return false;
        if (dependency.notPerformingDrawGun && drawGun.isPerformingAction)
            return false;

        if (dependency.notTurnedBack && turnback.turnedBack)
            return false;
        if (dependency.notTurning && turnback.isTurning)
            return false;

        if (dependency.notShooting && shoot.isShooting)
            return false;

        if (dependency.notWalking && move.isWalking)
            return false;

        if (dependency.notHiting && hit.isHiting)
            return false;

        return true;
    }

    float targetFov;
    Coroutine fovCoroutine;


    private void OnTriggerEnter(Collider other)
    {
        var smt = other.GetComponent<SceneModiferTrigger>();

        if (!(smt is null))
        {
            if (smt.modifyCamera_fov && cvc != null)
            {
                var lens = cvc.m_Lens;
                targetFov = smt.modifyCamera_fov_value;
                if (fovCoroutine != null)
                    StopCoroutine(fovCoroutine);

                fovCoroutine = StartCoroutine(ChangeCvcFov(smt.modifyCamera_fov_speed));
            }
        }
    }

    IEnumerator ChangeCvcFov(float speed)
    {
        while (true)
        {
            var crtFov = cvc.m_Lens.FieldOfView;
            if (Mathf.Abs(crtFov - targetFov) > 0.5f)
            {
                crtFov = Mathf.MoveTowards(crtFov, targetFov, Time.deltaTime * speed);
                var lens = cvc.m_Lens;
                lens.FieldOfView = crtFov;
                cvc.m_Lens = lens;
            }
            else
            {
                yield break;
            }

            yield return null;
        }
    }
}