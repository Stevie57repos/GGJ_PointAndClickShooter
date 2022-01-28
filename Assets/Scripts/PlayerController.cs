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

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _reload = _playerInput.actions["Reload"];

        _camera = GetComponentInChildren<Camera>();
        _nextAvailableLeftClick = float.MinValue;
        _nextAvailableRightClick = float.MinValue;
        SetupBulletsUI();

    }

    private void OnEnable()
    {
        _reload.performed += ReloadCheck;
    }

    private void OnDisable()
    {
        _reload.performed -= ReloadCheck;
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
            print($"reload pressed");
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

                FireBullet();
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

                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        UpdateBulletUI();

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        GunshotEffect(ray.direction * _gunRange);
        if (Physics.Raycast(ray, out var hit, _gunRange))
        {
            if (hit.transform.GetComponent<IInteractable>() != null)
            {
                hit.transform.GetComponent<IInteractable>().RightClick(_rightShotValue);
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
}
