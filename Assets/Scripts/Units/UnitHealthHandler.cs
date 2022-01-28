using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthHandler : HealthHandler
{
    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _image.color = Color.green;
    }
    protected override void UpdateHealthUI(float damage)
    {
        float healthPercent = _health / _maxHealth;
        _image.color = Color.Lerp(Color.red, Color.green, healthPercent);
    }
}
