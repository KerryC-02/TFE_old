using com;
using UnityEngine;

public class EnemyAnimationEventReceiver : MonoBehaviour
{
    public EnemyBehaviour host;


    public void OnFire()
    {
        host.attack.OnFire();

    }

    public void OnDrawGun()
    {
        SoundSystem.instance.Play("player reload", this.gameObject, 0.4f);
    }


    public void OnWalkStep()
    {
        SoundSystem.instance.Play(new string[] { "step1", "step2", "step3" }, this.gameObject, 1f);
    }

    public void OnRunStep()
    {
        SoundSystem.instance.Play(new string[] { "step run1", "step run2", "step run3", "step run4", "step run5" }, this.gameObject, 0.5f);
    }
}