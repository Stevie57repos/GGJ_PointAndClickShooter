using System.Collections;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [Tooltip("< 0 means infinite LifeSpan")]
    [SerializeField]
    protected float _lifeSpan = -1;

    protected WaitForSeconds _waitTime;
    protected Coroutine _disabler;

    protected virtual void OnEnable()
    {
        if (_lifeSpan >= 0)
        {
            _waitTime = new WaitForSeconds(_lifeSpan);
            _disabler = StartCoroutine(Disabler());
        }
    }

    protected IEnumerator Disabler()
    {
        yield return _waitTime;
        DisablePoolableObject();
    }

    public void DisablePoolableObject()
    {
        ClearCoroutine();
        gameObject.SetActive(false);
    }

    protected void ClearCoroutine()
    {
        if (_disabler == null) return;

        StopCoroutine(_disabler);
        _disabler = null;
    }

    protected void OnDisable()
    {
        ClearCoroutine();
    }
}
