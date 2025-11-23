using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int hpMax;
    public int hp { get; private set; }
    private EnemyBehaviour _host;

    private void Awake()
    {
        _host = GetComponent<EnemyBehaviour>();
    }
    void Start()
    {
        hp = hpMax;
    }


    public void TakeDamage(int d)
    {
        hp -= d;
        Debug.Log("TakeDamage " + d);
        if (hp <= 0)
            _host.death.Die();
        else
            _host.hit.TryHit();
    }
}