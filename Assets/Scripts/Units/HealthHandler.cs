using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField]
    protected float _health;
    protected float _minHealth = 0f;
    protected float _maxHealth;

    public void Setup(StatsSO stats)
    {
        _health = stats.HealthStats.maxHealth;
        _maxHealth = stats.HealthStats.maxHealth;
    }

    virtual public bool TakeDamage(float damage)
    {
        if (_health - damage <= _minHealth)
        {
            _health = _minHealth;
            UpdateHealthUI(damage);
            return false;
        }

        _health -= damage;
        // stopping over healing;
        if (_health > _maxHealth) _health = _maxHealth;

        UpdateHealthUI(damage);
        return true;
    }

    public bool HealthStatus()
    {
        return _health > 0;
    }

    protected virtual void UpdateHealthUI(float damage)
    {

    }
}
