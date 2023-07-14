using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] protected bool _invertScale;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _jumpSpeed;
    [SerializeField] private float _damageVelocity;


    [Header("Checkers")]
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerCheckComponent _groundCheck;
    [SerializeField] protected CheckCircleOvetlap _attackRange;
    [SerializeField] protected SpawnListComponent _particles;


    protected Rigidbody2D Rigidbody;
    protected Vector2 Direction;
    protected Animator Animator;
    protected PlaySoundsComponent Sounds;
    protected bool IsGrounded;
    protected bool _isJumping;
    protected HealthComponent _health;

    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    protected static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    protected static readonly int Hit = Animator.StringToHash("hit");
    protected static readonly int AttackKey = Animator.StringToHash("attack");
    protected static readonly int ThrowKey = Animator.StringToHash("throw");
    protected static readonly int DashKey = Animator.StringToHash("dash");
    protected static readonly int HurtKey = Animator.StringToHash("hurt");
    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Sounds = GetComponent<PlaySoundsComponent>();
        _health = GetComponent<HealthComponent>();
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    protected virtual void Update()
    {
        if (_groundCheck == null) return;
        IsGrounded = _groundCheck.IsTouchingLayer;
    }
    private void FixedUpdate()
    {
        var xVelocity = CalculateXVelocity();
        var yVelocity = CalculateYVelocity();
        if (Rigidbody == null) return;
        Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        if (Animator == null) return;
        Animator.SetBool(IsGroundKey, IsGrounded);
        Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
        Animator.SetBool(IsRunning, Direction.x != 0);

        UpdateSpriteDirection(Direction);

    }
    protected virtual float CalculateXVelocity()
    {
        return Direction.x * CalculateSpeed();
    }

    protected virtual float CalculateSpeed()
    {
        return _speed;
    }
    protected virtual float CalculateYVelocity()
    {
    
        var yVelocity = Rigidbody.velocity.y;

        var isJumpPressing = Direction.y > 0;
        if (IsGrounded)
        {
            _isJumping = false;
        }

        if (isJumpPressing)
        {
            _isJumping = true;

            var isFalling = Rigidbody.velocity.y <= 0.001f;
            yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
        }

        else if (Rigidbody.velocity.y > 0 && _isJumping)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }



    protected virtual float CalculateJumpVelocity(float yVelocity)
    {
        if (IsGrounded)
        {
            yVelocity = _jumpSpeed;
            DoJumpVfx();

        }

        return yVelocity;
    }

    protected void DoJumpVfx()
    {
        if (_particles == null) return;
        _particles.Spawn("Jump");
        Sounds.Play("Jump");
    }
    public virtual void UpdateSpriteDirection(Vector2 direction)
    {
        var multiplayer = _invertScale ? -3 : 3;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(multiplayer, 3, 3);

        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1 * multiplayer, 3, 3);
        }

    }
    //  private bool IsGrounded()
    //  {
    //   return _groundCheck.IsTouchingLayer;

    // }
    public virtual void TakeDamage(int damage)

    {
      
        _health.TakeDmg(damage);
        _isJumping = false;
        if (Animator == null) return;
        Animator.SetTrigger(Hit);
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        
    }
    public virtual void Attack()
    {
        if (Animator == null) return;
        Animator.SetTrigger(AttackKey);
        Sounds.Play("Melee");
    }
    public void OnAttackEnemy()
    {
        if (_attackRange == null) return;
        _attackRange.Check();


    }


}
