using UnityEngine;

public class LoopingMove : MonoBehaviour
{
    public float speed = 1.1f;        // 移动速度

    // private Vector3 originalPosition; // 记录初始位置

    public float spawnDistance;
    public float DisappearDistance;


    void Start()
    {
        // originalPosition = transform.position; // 在开始时获取并存储初始位置
    }

    void Update()
    {

        var playerPosX = PlayerBehaviour.instance.transform.position.x;
        var newX = transform.position.x;
        var deltaX = newX - playerPosX;
        var needToMove = false;

        if (-deltaX > DisappearDistance)
        {
            //我在玩家左边很多，复位我的位置
            newX = playerPosX + spawnDistance;
        }

        if (deltaX > spawnDistance)
        {
            //我在玩家右边很多,我还不该走路
            needToMove = false;
        }
        else
        {
            //我在玩家附近
            needToMove = true;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (needToMove)
            transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
