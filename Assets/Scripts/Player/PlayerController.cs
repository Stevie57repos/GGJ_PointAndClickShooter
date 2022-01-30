using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
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

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _gunShotClip;
    [SerializeField]
    private AudioClip _dartShotClip;
    [SerializeField]
    private AudioClip _reloadClip;
    private bool _reloadclipPlayed = false;
    [SerializeField]
    private AudioClip _emptyClip;
    private bool _emptyClipPlayed = false;
    [SerializeField]
    private AudioClip _playerHurtClip;
    [SerializeField]
    private AudioSource _bgAudio;
    [SerializeField]
    private AudioSource _bgAmbientAudio;

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
    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;
    [SerializeField]
    private StopSoundEventChannelSO _stopSoundEventChannel;

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
        _stopSoundEventChannel.StopSoundEvent += StopAudio;
    }

    private void OnDisable()
    {
        _reload.performed -= ReloadCheck;
        _allyDeathEventChannel.AllyDeathEvent -= AllyDeath;
        _stopSoundEventChannel.StopSoundEvent -= StopAudio;
    }

    private void Update()
    {
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
            if(!_reloadclipPlayed)
            {
                _soundEventChannel.RaiseEvent(_reloadClip, this.transform);
                _reloadclipPlayed = true;
                StartCoroutine(ReloadingRoutine());
            }
        }
    }

    private IEnumerator ReloadingRoutine()
    {
        yield return new WaitForSeconds(1.1f);
        _reloadclipPlayed = false;
    }

    private void LeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            if (CheckLeftClick() && CheckBulletCount())
            {
                _nextAvailableLeftClick = Time.time + _fireRateCoolDown;

                FireBullet(_leftShotValue);
                _soundEventChannel.RaiseEvent(_gunShotClip, this.transform);
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
                _soundEventChannel.RaiseEvent(_dartShotClip, this.transform);
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

    private int RandomNumber(int limit)
    {
        int randomNumber = 0;
        int lastNumber = 0;
        int maxAttempts = 5;

        for(int i = 0; randomNumber == lastNumber && i < maxAttempts; i++)
        {
            randomNumber = UnityEngine.Random.Range(0, limit);
        }

        return randomNumber; 
    }

    private bool CheckBulletCount()
    {
        bool canShoot = _currentBullets > 0;
        if (!canShoot)
        {
            if (!_emptyClipPlayed)
            {
                _soundEventChannel.RaiseEvent(_emptyClip, this.transform);
                _emptyClipPlayed = true;
                StartCoroutine(EmptyClipRoutine());
            }
        }
        return canShoot;
    }

    private IEnumerator EmptyClipRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        _emptyClipPlayed = false;
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
        _soundEventChannel.RaiseEvent(_playerHurtClip, this.transform);

        if (!isAlive)
            _loseEventChannel.RaiseEvent();
    }

    public void StopAudio()
    {
        _bgAmbientAudio.Stop();
        _bgAudio.Stop();
    }
}
