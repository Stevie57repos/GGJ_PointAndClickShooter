using System.Collections;
using UnityEngine;

public class SoundEmitter : PoolableObject
{
    private Transform _soundTransform = null;
    private AudioSource _audioSource;

    public void PlayOneShot(AudioClip sound, Transform spawnPoint)
    {
        _soundTransform = spawnPoint;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(sound);
        StartCoroutine(DeactivateAfterClip(spawnPoint));
    }

    private IEnumerator DeactivateAfterClip(Transform spawnPoint)
    {
        if (_audioSource.isPlaying)
        {
            transform.position = spawnPoint.position;
            yield return null;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
