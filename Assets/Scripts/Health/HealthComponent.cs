using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHP;

    [SerializeField] public UnityEvent _onDie;
    [SerializeField] public HealthChangeEvent _onChange;

    [SerializeField] private bool _immune;
   

    public Action<int, int> OnHealthChange;

    public int HP => _hp;
    public int MaxHP => _maxHP;

    public bool Immune
    {
        get => _immune; 
        set => _immune = value;
    }

    
    public void Initialized(int HP, int MaxHP)
    {
        _hp = HP;
        _maxHP = MaxHP;

    }


    public void TakeDmg(int healthDelta)
    {       
        if (healthDelta < 0 && Immune ) return; 
        if (_hp <= 0) return;

        _hp += healthDelta;
        _onChange?.Invoke(_hp);
        OnHealthChange?.Invoke(HP, MaxHP);

        if (_hp <= 0)
        {
            _onDie!?.Invoke();
        }
    }

    public void IncreaseHp(int healthDelta)
    {
        _maxHP += healthDelta;
        _hp += healthDelta;

        _onChange?.Invoke(_hp);
        OnHealthChange?.Invoke(HP, MaxHP);
    }

    public void AddedHp(int healthDelta)
    {
        int tmp = _hp + healthDelta;

        if(tmp >= MaxHP)
            _hp = MaxHP;
        else
            _hp = tmp;
        _onChange?.Invoke(_hp);
        OnHealthChange?.Invoke(HP, MaxHP);
    }

#if UNITY_EDITOR
    [ContextMenu("Update Health")]
    private void UpdateHealth()
    {
        _onChange?.Invoke(_hp);
    }
#endif




    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
       
    }


    /////////////////// OBSOLETE  //////////////////////////////////
    //public void SetHealth(int health)
    //  {
    //    _health = health;
    // }
}
