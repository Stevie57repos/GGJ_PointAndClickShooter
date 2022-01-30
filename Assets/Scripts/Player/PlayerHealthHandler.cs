using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : HealthHandler
{
    [SerializeField]
    List<GameObject> _healthUIList = new List<GameObject>();

    [Header("Debugging")]
    [SerializeField]
    private bool _isInvincible;
    [SerializeField]
    private GameObject _playerHurtUI;
    [SerializeField]
    private float _hurtIndicatorDuration;

    private void Awake()
    {
        _playerHurtUI.SetActive(false); 
    }

    protected override void UpdateHealthUI(float damage)
    {
        if(damage > 0)
        {
            StartCoroutine(PlayerDamageUIRoutine());
            if(_health <= 0)
            {
                _healthUIList[(int)_health].SetActive(false);
                return;
            }
            else
                _healthUIList[(int)_health].SetActive(false);           
        }
        // healing
        else if(damage > 0 && _health < _maxHealth + 1)
        {
            for (int i = 0; i < _healthUIList.Count; i++)
            {
                if(!_healthUIList[i].activeInHierarchy)
                    _healthUIList[i].SetActive(true);
            }
        }
    }

    public override bool TakeDamage(float damage)
    {
        if(!_isInvincible) return base.TakeDamage(damage);

        return true;
    }

    private IEnumerator PlayerDamageUIRoutine()
    {
        _playerHurtUI.SetActive(true);
        yield return new WaitForSeconds(_hurtIndicatorDuration);
        _playerHurtUI.SetActive(false);
    }
}
