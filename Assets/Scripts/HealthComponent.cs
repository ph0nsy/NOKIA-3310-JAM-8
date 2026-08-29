using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [HideInInspector]
    public int MaxHP { get; set; }
    public int CurrentHP;

    public event Action<int, bool> OnHealthChanged;
    public event Action OnDeath;

    public void Init(int _maxHP, int _currentHP)
    {
        MaxHP = _maxHP;
        CurrentHP = _currentHP;
    }

    public void Damage(int _amount)
    {
        
        CurrentHP -= _amount;
        CurrentHP = Mathf.Max(0, CurrentHP);

        OnHealthChanged?.Invoke(CurrentHP, false);

        if (CurrentHP == 0) { OnDeath?.Invoke(); }
    }

    public void Heal(int amount)
    {
        if (CurrentHP <= 0) return;

        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
        OnHealthChanged?.Invoke(CurrentHP, true);
    }
}