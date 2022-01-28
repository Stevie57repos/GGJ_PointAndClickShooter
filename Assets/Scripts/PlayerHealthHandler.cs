using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : HealthHandler
{
    [SerializeField]
    List<GameObject> _healthUIList = new List<GameObject>();

    protected override void UpdateHealthUI(float damage)
    {
        if(damage > 0)
        {
            if(_health <= 0)
            {
                _healthUIList[(int)_health].SetActive(false);
                return;
            }
            else
                _healthUIList[(int)_health].SetActive(false);           
        }
        else if(damage > 0 && _health < _maxHealth + 1)
        {
            for (int i = 0; i < _healthUIList.Count; i++)
            {
                if(!_healthUIList[i].activeInHierarchy)
                    _healthUIList[i].SetActive(true);
            }
        }
    }
}
