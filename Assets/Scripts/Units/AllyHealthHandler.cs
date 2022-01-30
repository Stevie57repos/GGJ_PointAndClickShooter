using UnityEngine;

public class AllyHealthHandler : UnitHealthHandler
{
    [SerializeField]
    private float _startingHealth;
    public override void Setup(StatsSO stats)
    {
        _health = _startingHealth;
        _maxHealth = stats.HealthStats.maxHealth;
        UpdateHealthUI(0f);      
    }
}
