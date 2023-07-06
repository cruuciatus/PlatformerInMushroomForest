using System.Collections;
using UnityEngine;

public class Hero : Creature
{
    [SerializeField] private Vector2 _interactionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _slamDownVelocity;
    [SerializeField] private LayerCheckComponent _wallCheck;

    [SerializeField] private CheckCircleOvetlap _interactionCheck;

    [Header("SuperThrow")]
    [SerializeField] private Cooldown _superThrowCooldown;
    [SerializeField] private int _superThrowParticles;
    [SerializeField] private float _superThrowDelay;

    [SerializeField] private Cooldown _throwCoolDown;
    [SerializeField] private RuntimeAnimatorController _armed;
    [SerializeField] private RuntimeAnimatorController _unarmed;
    [SerializeField] private float _dashDelta;
    [SerializeField] private SpawnComponent _throwSpawner;
    [SerializeField] private ShieldComponent _shield;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _hitParticles;

    private int CoinsCount => _session.Data.Inventory.Count("Coin");
    private int SwordCount => _session.Data.Inventory.Count(SwordId);

    private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
    private const string SwordId = "Sword";

    private bool CanThrow
    {
        get
        {
            if (SelectedItemId == SwordId)
            {
                return SwordCount > 1;
            }
            var def = DefsFacade.I.Items.Get(SelectedItemId);
            return def.HasTag(ItemTag.Throwable);

        }
    }


    private Collider2D[] _interactionResults = new Collider2D[1];
    private HealthComponent _health;
    private bool _allowDoubleJump;
    private float _defaultGravityScale;
    private GameSession _session;
    private bool _isOnWall;
    private bool _superThrow;

    protected override void Awake()
    {
        base.Awake();
        _defaultGravityScale = Rigidbody.gravityScale;

    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _health = GetComponent<HealthComponent>();

        //  _session.Data.Inventory.OnChanged += OnInventoryChanged;
        _session.StatsModel.OnUpgraded += OnHeroUpgraded;


        // _health.SetHealth(_session.Data.HP.Value);
        _health.Initialized(_session.Data.HP.Value, _session.Data.MaxHP.Value);
        //  UpdateHeroWeapon();

      
    }

    private void OnHeroUpgraded(StatId statId)
    {
        switch (statId)
        {
            case StatId.Hp:
                int addedHp = (int)_session.StatsModel.GetValue(statId);
                _health.IncreaseHp(addedHp);
                _session.Data.HP.Value = _health.HP;
                _session.Data.MaxHP.Value = _health.MaxHP;
                _session.Save();
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
        var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
        if (_wallCheck.IsTouchingLayer && moveToSameDirection)
        {
            _isOnWall = true;
            Rigidbody.gravityScale = 0;
        }
        else
        {
            _isOnWall = false;
            Rigidbody.gravityScale = _defaultGravityScale;
        }

        if (isStartThrow)
        {
            delayThrow += Time.deltaTime;
        }

    }

    protected override float CalculateXVelocity()
    {
        return base.CalculateXVelocity() + Rigidbody.velocity.x/10;
    }

    protected override float CalculateYVelocity()
    {
        var _isJumpPressing = Direction.y > 0;

        if (IsGrounded || _isOnWall)
        {
            _allowDoubleJump = true;

        }

        if (!_isJumpPressing && _isOnWall)
        {
            return 0f;
        }

        return base.CalculateYVelocity();
    }

    protected override float CalculateJumpVelocity(float yVelocity)
    {

        if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall)
        {
            _session.PerksModel.Cooldown.Reset();
            _allowDoubleJump = false;
            DoJumpVfx();
            return _jumpSpeed;
        }

        return base.CalculateJumpVelocity(yVelocity);
    }



    public void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
        _session.Data.Inventory.Remove("Coin", numCoinsToDispose);


        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);

        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }

    public void Interact()
    {
        _interactionCheck.Check();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.IsLayer(_groundLayer))
        {
            var contact = other.contacts[0];
            if (contact.relativeVelocity.y >= _slamDownVelocity)
            {
                _particles.Spawn("SlamDown");

            }
        }
    }

    public override void UpdateSpriteDirection(Vector2 direction)
    {
        var multiplayer = _invertScale ? -2 : 2;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(multiplayer, 2, 2);

        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1 * multiplayer, 2, 2);
        }
    }


    ///////////////////  ATTACK////////////////////////////////////

    public override void Attack()
    {
        
        // if (SwordCount <= 0) return;
        base.Attack();

    }

    public override void TakeDamage(int damage)
    {

        base.TakeDamage(damage);
        _health.TakeDmg(damage);
        Sounds.Play("Hurt");
        if (CoinsCount > 0)
        {
            SpawnCoins();
        }
       
    }

    public void OnDoThrow()
    {
        if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
        {
            Animator.SetTrigger(ThrowKey);
            var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
            var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;

            var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
            _session.PerksModel.Cooldown.Reset();
            StartCoroutine(DoSuperThrow(numThrows));
        }
        else
        {
            ThrowAndRemoveFromInventory();
        }
        _superThrow = false;
    }

    private IEnumerator DoSuperThrow(int numThrows)
    {
        for (int i = 0; i < numThrows; i++)
        {
            ThrowAndRemoveFromInventory();
            yield return new WaitForSeconds(_superThrowDelay);
        }
    }
    public void ThrowAndRemoveFromInventory()
    {
        Sounds.Play("Range");

        var throwableId = _session.QuickInventory.SelectedItem.Id;
        var throwwableDef = DefsFacade.I.Throwable.Get(throwableId);
        _throwSpawner.SetPrefab(throwwableDef.Projectile);
        var instance = _throwSpawner.SpawnInstance();
        ApplyRangeDamageStat(instance);

        _session.Data.Inventory.Remove(throwableId, 1);
    }


    private void ApplyRangeDamageStat(GameObject projectile)
    {
        var hpModify = projectile.GetComponent<ModifyHealthComponent>();
        var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
        damageValue = ModifyDamageByCrit(damageValue);
        hpModify.SetDelta(-damageValue);
    }

    private int ModifyDamageByCrit(int damage)
    {
        var critChange = _session.StatsModel.GetValue(StatId.CriticalDamage);
        if (Random.value * 100 <= critChange)
        {
            return damage * 2;
        }
        return damage;
    }
    ///////////////////////////////////////////////////////


    ///////////////////INVENTORY////////////////////////////////////
    public void Throw()
    {
        if (_throwCoolDown.IsReady && SwordCount > 1)
        {
            Animator.SetTrigger(ThrowKey);
            _throwCoolDown.Reset();
        }
    }

    bool isStartThrow;
    float delayThrow;
    public void StartTimerSuperThrow()
    {
        isStartThrow = true;
    }
    public void UseInventory()
    {

        if (isSelectedItem(ItemTag.Throwable))
        {
            if (delayThrow > 0.5 && _session.PerksModel.IsSuperThrowSupported)
            {
                Animator.SetTrigger(ThrowKey);
                Invoke(nameof(OnDoThrow), 0.1f);
                //Invoke(nameof(OnDoThrow), 0.3f);
                // Invoke(nameof(OnDoThrow), 0.6f);
            } else
            {

                PerformThrowling();
            }

        }
        else if (isSelectedItem(ItemTag.Potion))
        {
            UsePotion();
        }

        isStartThrow = false;
        delayThrow = 0;
    }
    
    private void UsePotion()
    {
        var potion = DefsFacade.I.Potions.Get(SelectedItemId);

        switch (potion.Effect)
        {
            case Effect.AddHp:
                _health.AddedHp((int)potion.Value);
                break;
            case Effect.SpeedUp:
                _speedUpColldown.Value = _speedUpColldown.RemainingTime + potion.Time;
                _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                _speedUpColldown.Reset();
                break;
        }

        _session.Data.Inventory.Remove(potion.Id, 1);
    }
    public void AddInInventory(string id, int value)
    {
        _session.Data.Inventory.Add(id, value);
    }
    ///////////////////////////////////////////////////////


    private readonly Cooldown _speedUpColldown = new Cooldown();
    private float _additionalSpeed;


    protected override float CalculateSpeed()
    {
        if (_speedUpColldown.IsReady)
        {
            _additionalSpeed = 0f;
        }
        var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
        return defaultSpeed;
        // return base.CalculateSpeed() + _additionalSpeed;
    }

    [SerializeField] private int DashImpulse;
    public bool faceRight = true;
   //[SerializeField] private TrailRenderer tr;
    public void Dash()
    {
        if (_session.PerksModel.IsDashSupported)
        {
            _session.PerksModel.Cooldown.Reset();
            Animator.SetTrigger("dash");
            Rigidbody.velocity = new Vector2(0, 0);
            if (Rigidbody.transform.localScale.x < 0)
            {
               Rigidbody.AddForce(Vector2.left * DashImpulse, ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody.AddForce(Vector2.right * DashImpulse, ForceMode2D.Impulse);
            }  
        }
    }


    private bool isSelectedItem(ItemTag tag)
    {
        return _session.QuickInventory.SelectedDef.HasTag(tag);
    }





    public void PerformThrowling()
    {
        if (!_throwCoolDown.IsReady || !CanThrow) return;

        if (_superThrowCooldown.IsReady) _superThrow = true;
        Animator.SetTrigger(ThrowKey);
        _throwCoolDown.Reset();
    }

    public void NextItem()
    {
        _session.QuickInventory.SetNextItem();
    }

    public void UsePerk()
    {
        if (_session.PerksModel.IsShieldSupported)
        {
            _shield.Use();
            _session.PerksModel.Cooldown.Reset();
        }
    }



    //  private void UpdateHeroWeapon()
    // {
    //   Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
    // }

    //public void StartThrowing()
    //{
    // _superThrowCooldown.Reset();
    //}


    // private void OnDestroy()
    // {
    //   _session.Data.Inventory.OnChanged -= OnInventoryChanged;

    // }
    // private void OnInventoryChanged(string id, int value)
    // {
    //if (id == SwordId)
    // {
    //    UpdateHeroWeapon();
    // }
    // }

    // public void OnHealthChange(int currentHealth)
    // {
    //    _session.Data.HP.Value = currentHealth;
    //}


}

