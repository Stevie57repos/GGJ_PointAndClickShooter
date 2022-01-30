using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { EnemyTakeDamage, EnemyLaser};

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private PoolableObject _soundEmitterPrefab;

    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;

    private void Awake()
    {
        PoolSystem.CreatePool(_soundEmitterPrefab, 10);
    }

    private void OnEnable()
    {
        _soundEventChannel.SoundEvent += PositionAndPlaySoundEmitter;
    }

    private void OnDisable()
    {
        _soundEventChannel.SoundEvent -= PositionAndPlaySoundEmitter;
    }

    private void PositionAndPlaySoundEmitter(AudioClip soundClip, Transform spawnPoint)
    {
        SoundEmitter soundEmitter = PoolSystem.GetNext(_soundEmitterPrefab) as SoundEmitter;
        soundEmitter.gameObject.SetActive(true);
        soundEmitter.PlayOneShot(soundClip, spawnPoint);
    }
}
