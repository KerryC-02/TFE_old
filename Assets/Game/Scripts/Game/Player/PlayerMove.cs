using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    SpriteRenderer _sr;
    public enum HorizontalMoveState
    {
        None,
        Right,
        Left,
    }

    public HorizontalMoveState horizontalMoveState { get; private set; }

    public KeyCode walkRightKey = KeyCode.D;
    public KeyCode walkLeftKey = KeyCode.A;
    bool _walkRightIsPressed;

    bool _walkLeftIsPressed;
    bool _shiftIsPressed;

    public Transform flipTrans;
    public Transform characterTrans;
    public float moveSpeed;
    public float runExtraSpeed;

    bool _lastMoved;
    public PlayerActionPerformDependency dependency;
    private Rigidbody _rb;

    private void Awake()
    {
        _sr = flipTrans.GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        horizontalMoveState = HorizontalMoveState.None;
        FlipRight();
        PlayerBehaviour.instance.animator.SetInteger("walk style", 1);
    }

    // Update is called once per frame
    void Update()
    {
          _rb.linearVelocity = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(walkRightKey))
            _walkRightIsPressed = true;
        if (Input.GetKeyDown(walkLeftKey))
            _walkLeftIsPressed = true;
        if (Input.GetKeyUp(walkRightKey))
            _walkRightIsPressed = false;
        if (Input.GetKeyUp(walkLeftKey))
            _walkLeftIsPressed = false;


        if (Input.GetKeyDown(KeyCode.LeftShift))
            _shiftIsPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _shiftIsPressed = false;

        if (!PlayerBehaviour.instance.AssertCondition(dependency))
            return;

        Move();
    }

    private void Move()
    {
        bool currentMoved = false;
        if ((_walkLeftIsPressed && _walkRightIsPressed) || (!_walkLeftIsPressed && !_walkRightIsPressed))
        {
            //stop move
            if (horizontalMoveState != HorizontalMoveState.None)
            {
                PlayerBehaviour.instance.animator.SetBool("walk", false);
                horizontalMoveState = HorizontalMoveState.None;
            }
        }
        else
        {
            currentMoved = true;
            var finalSpeed = moveSpeed;
            if (!PlayerBehaviour.instance.drawGun.hasGunDrawn)
            {
                if (_shiftIsPressed)
                {
                    finalSpeed += runExtraSpeed;
                    PlayerBehaviour.instance.animator.SetBool("shift", true);
                }
                else
                {
                    PlayerBehaviour.instance.animator.SetBool("shift", false);
                }
            }

            if (_walkLeftIsPressed)
            {
                //move left
                if (horizontalMoveState != HorizontalMoveState.Left)
                {
                    FlipLeft();
                    //PlayerBehaviour.instance.animator.SetInteger("walk style", UnityEngine.Random.Range(1, 3));
                    PlayerBehaviour.instance.animator.SetBool("walk", true);
                    horizontalMoveState = HorizontalMoveState.Left;
                }

                //characterTrans.position += finalSpeed * Time.deltaTime * new Vector3(-1, 0, 0);
                _rb.linearVelocity = new Vector3(-finalSpeed, 0, 0);
            }
            else if (_walkRightIsPressed)
            {
                //move right
                if (horizontalMoveState != HorizontalMoveState.Right)
                {
                    FlipRight();
                    //PlayerBehaviour.instance.animator.SetInteger("walk style", UnityEngine.Random.Range(1, 3));
                    PlayerBehaviour.instance.animator.SetBool("walk", true);
                    horizontalMoveState = HorizontalMoveState.Right;
                }
                //characterTrans.position += finalSpeed * Time.deltaTime * new Vector3(1, 0, 0);
                _rb.linearVelocity = new Vector3(finalSpeed, 0, 0);
            }
        }

        if (_lastMoved != currentMoved)
            _lastMoved = currentMoved;
    }

    public bool isWalking
    {
        get
        {
            return _lastMoved;
        }
    }

    public void FlipRight()
    {
        //flipTrans.localScale = new Vector3(1, 1, 1);
        _sr.flipX = false;
    }

    public void FlipLeft()
    {
        // flipTrans.localScale = new Vector3(-1, 1, 1);
        _sr.flipX = true;
    }

    public bool isFacingRight
    {
        get { return !_sr.flipX; }
    }
}
