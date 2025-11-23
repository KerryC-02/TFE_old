using com;
using UnityEngine;

public class PlayerAnimationEventReceiver : MonoBehaviour
{
    public PlayerBehaviour host;


    public void OnFire()
    {
        host.shoot.OnFire();
        SoundSystem.instance.Play("player gun");
    }

    public void OnDrawGun()
    {
        SoundSystem.instance.Play("player reload");
    }


    public void OnWalkStep()
    {
        SoundSystem.instance.Play(new string[] { "step1", "step2", "step3" });
    }

    public void OnRunStep()
    {
        SoundSystem.instance.Play(new string[] { "step run1", "step run2", "step run3", "step run4", "step run5" });
    }
}