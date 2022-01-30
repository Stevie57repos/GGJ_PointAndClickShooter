using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _leftClick;
    private InputAction _mouse;
    private InputAction _reload;
    private Camera _camera;

    [Header("Gun Settings")]
    [Range(0.1f, 300f)]
    [SerializeField]
    private float _gunRange;
    [SerializeField]
    private float _fireRateCoolDown;
    [SerializeField]
    private float _reloadTime;
    private float _nextAvailableLeftClick;
    [SerializeField]
    private float _leftShotValue;
    private float _nextAvailableRightClick;
    [SerializeField]
    private float _rightShotValue;
    [SerializeField]
    private GameObject _gun;
    [SerializeField]
    private Transform _mouseTarget;
    [SerializeField]
    private ParticleSystem _gunshotParticle;

    [Header("Bullets Settings")]
    [SerializeField]
    private int _maxBullets;
    [SerializeField]
    private int _currentBullets;
    [SerializeField]
    List<GameObject> _bulletsList = new List<GameObject>();
    [SerializeField]
    private Transform BulletsParentUI;
    [SerializeField]
    private GameObject BulletsUI;

    [Header("Health Settings")]
    [SerializeField]
    private PlayerHealthHandler _healthHandler;
    [SerializeField]
    private StatsSO _statsSO;

    [Header("Event Channel")]
    [SerializeField]
    private PlayerLoseEventChannel _loseEventChannel;
    [SerializeField]
    private AllyDeathEventSO _allyDeathEventChannel;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _reload = _playerInput.actions["Reload"];

        _camera = GetComponentInChildren<Camera>();
        _nextAvailableLeftClick = float.MinValue;
        _nextAvailableRightClick = float.MinValue;

        _healthHandler.Setup(_statsSO);
    }

    private void Start()
    {
        SetupBulletsUI();
    }

    private void OnEnable()
    {
        _reload.performed += ReloadCheck;
        _allyDeathEventChannel.AllyDeathEvent += AllyDeath;
    }

    private void OnDisable()
    {
        _reload.performed -= ReloadCheck;
        _allyDeathEventChannel.AllyDeathEvent -= AllyDeath;
    }

    private void Update()
    {
        //PointGun();
        LeftClick();
        RightClick();
    }

    private void SetupBulletsUI()
    {
        _currentBullets = _maxBullets;
        for(int i = 0; i < _maxBullets; i++)
        {
            GameObject bullet = Instantiate(BulletsUI, BulletsParentUI);
            _bulletsList.Add(bullet);
        }
    }

    private void ReloadCheck(InputAction.CallbackContext context)
    {
        bool isReloading = context.ReadValueAsButton();
        if (isReloading)
        {
            _nextAvailableLeftClick = Time.time + _reloadTime;
            foreach(var bullet in _bulletsList)
            {
                if (!bullet.activeInHierarchy) bullet.SetActive(true); 
            }
            _currentBullets = _maxBullets;
            // play reload sound
        }
    }

    // TO DO
    //private void PointGun()
    //{
    //    var ray = _camera.ScreenPointToRay(Input.mousePosition);
    //    if(Physics.Raycast(ray, out var hit, 300f))
    //    {
    //        if (hit.collider)
    //        {
    //            _mouseTarget.position = hit.collider.transform.position;
    //        }
    //    }

    //    _gun.transform.LookAt(_mouseTarget);
    //}

    private void LeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            if (CheckLeftClick() && CheckBulletCount())
            {
                _nextAvailableLeftClick = Time.time + _fireRateCoolDown;

                FireBullet(_leftShotValue);
            }
        }
    }

    private void RightClick()
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckRightClick() && CheckBulletCount())
            {
                _nextAvailableRightClick = Time.time + _fireRateCoolDown;

                FireBullet(_rightShotValue);
            }
        }
    }

    private void FireBullet(float shotValue)
    {
        UpdateBulletUI();

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        GunshotEffect(ray.direction * _gunRange);
        if (Physics.Raycast(ray, out var hit, _gunRange))
        {
            if (hit.transform.GetComponent<IInteractable>() != null)
            {
                hit.transform.GetComponent<IInteractable>().RightClick(shotValue);
            }
        }
    }

    private void UpdateBulletUI()
    {
        if (_currentBullets - 1 < 0) return;
        _bulletsList[_currentBullets - 1].SetActive(false);
        _currentBullets--;
    }

    private bool CheckLeftClick()
    {
        bool isAvailable = false;

        if (Time.time > _nextAvailableLeftClick)
        {
            isAvailable = true;
        }
        return isAvailable;
    }

    private bool CheckRightClick()
    {
        bool isAvailable = false;

        if (Time.time > _nextAvailableRightClick)
        {
            isAvailable = true;
        }
        return isAvailable;
    }

    private void GunshotEffect(Vector3 targetPos)
    {
        _gunshotParticle.gameObject.transform.forward = targetPos;
        _gunshotParticle.Play();
    }

    private bool CheckBulletCount()
    {
        return _currentBullets > 0;
    }

    [ContextMenu("Take 1 damage")]
    public void DebugTestPlayerDamage()
    {
        _healthHandler.TakeDamage(1f);
    }

    private void AllyDeath()
    {
        TakeDamage(1f);
    }

    public void TakeDamage(float damage)
    {
       bool isAlive = _healthHandler.TakeDamage(damage);
        if (!isAlive)
            _loseEventChannel.RaiseEvent();
    }
}
