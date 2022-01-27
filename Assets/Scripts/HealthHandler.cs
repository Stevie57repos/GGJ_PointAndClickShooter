using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField]
    private float _health;
    private float _minHealth = 0f;
    private float _maxHealth;
    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _image.color = Color.green;
    }

    public void Setup(StatsSO stats)
    {
        _health = stats.HealthStats.maxHealth;
        _maxHealth = stats.HealthStats.maxHealth;
    }

    public bool TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= _minHealth)
        {
            _health = _minHealth;
            return false;
        }

        // stopping over healing;
        if(_health > _maxHealth) _health = _maxHealth;
        float healthPercent = _health / _maxHealth;
        _image.color = Color.Lerp(Color.red, Color.green, healthPercent);
        return true;
    }

    public bool HealthStatus()
    {
        return _health > 0;
    }
}
