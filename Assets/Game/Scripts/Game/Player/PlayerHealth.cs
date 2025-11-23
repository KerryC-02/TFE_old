using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int hpMax;
    public int hp { get; private set; }


    void Start()
    {
        hp = hpMax;
    }

    public void TakeDamage(int d)
    {
        hp -= d;
        Debug.Log("player TakeDamage rest hp " + hp);
        if (hp <= 0)
            PlayerBehaviour.instance.death.Die();
        else
            PlayerBehaviour.instance.hit.TryHit();
    }
}