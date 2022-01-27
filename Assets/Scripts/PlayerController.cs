using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _leftClick;
    private InputAction _mouse;
    private Camera _camera;

    [Range(0.1f, 300f)]
    [SerializeField]
    private float _gunRange;
    [SerializeField]
    private float _fireRateCoolDown;
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

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _nextAvailableLeftClick = float.MinValue;
        _nextAvailableRightClick = float.MinValue;
    }

    private void Update()
    {
        //PointGun();
        LeftClick();
        RightClick();
    }

    private void PointGun()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit, 300f))
        {
            if (hit.collider)
            {
                _mouseTarget.position = hit.collider.transform.position;
            }
        }

        _gun.transform.LookAt(_mouseTarget);
    }

    private void LeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            if (CheckLeftClick())
            {

                _nextAvailableLeftClick = Time.time + _fireRateCoolDown;
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                GunshotEffect(ray.direction * _gunRange);
                if (Physics.Raycast(ray, out var hit, _gunRange))
                {
                    if (hit.transform.GetComponent<IInteractable>() != null)
                    {
                        hit.transform.GetComponent<IInteractable>().LeftClick(_leftShotValue);
                    }
                }
            }
        }
    }

    private void RightClick()
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckRightClick())
            {

                _nextAvailableRightClick = Time.time + _fireRateCoolDown;
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
        }
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
}
