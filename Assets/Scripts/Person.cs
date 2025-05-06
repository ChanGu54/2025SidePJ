using UnityEngine;

public class Person : Entity
{
    [SerializeField] protected float speed;
    [SerializeField] protected Animator animator;
    protected bool isFacingRight = true;
    public float Speed => speed;
    public Animator Animator => animator;
    private Rigidbody2D _rb;
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 3f; // 중력 설정
        _rb.freezeRotation = true; // 회전 방지
    }

    protected virtual void Filp()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void Move(float horizontalInput)
    {
        _rb.linearVelocity = new Vector2(horizontalInput * speed, _rb.linearVelocity.y);

        //방향 전환
        if (horizontalInput > 0 && !isFacingRight)
        {
            Filp();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Filp();
        }

        if (animator != null)
        {
            float speed = Mathf.Abs(horizontalInput);
            Debug.Log(speed);
            animator.SetFloat(SpeedHash, speed);
        }
    }
}
