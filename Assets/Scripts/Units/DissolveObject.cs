using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveObject : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    [SerializeField]
    private Material material;

    [SerializeField]
    private float _minValue = 2.3f;
    private float _maxValue = 4.3f;
    private float _startTime;
    [SerializeField]
    private float _dissolvePercentage;

    [Header("Debugging")]
    private bool _isLoop = true;
    private bool _isIn = false;  
    [Range(0, 4)]
    [SerializeField]
    private float testValues;

    private  IEnumerator TestDissolveOut()
    {
        yield return new WaitForSeconds(2f);
        DissolveOut(0.5f);
    }

    private IEnumerator TestingDissolve() 
    {
        while (_isLoop)
        {
            if (!_isIn)
            {
                _isIn = true;
                DissolveIn(.5f);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                _isIn = false;
                DissolveOut(.5f);
                yield return new WaitForSeconds(1f);                
            }    
        }
    }

    private void SetHeight(float height)
    {
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }

    private void Update()
    {
        //material = GetComponent<Renderer>().material;           
        //SetHeight(testValues);
    }

    public void DissolveIn(float dissolveTime)
    {
        material = GetComponent<Renderer>().material;
        _dissolvePercentage = 0;
        _startTime = Time.time;
        _minValue = (float)transform.position.y - 1.1f;
        _maxValue = (float)transform.position.y + 1.1f;

        StartCoroutine(DissolveInRoutine(dissolveTime));
    }

    private IEnumerator DissolveInRoutine(float dissolveTime)
    {
        while (_dissolvePercentage < 1f)
        {
            _dissolvePercentage = (Time.time - _startTime) / dissolveTime;
            float newValue = Mathf.Lerp(_minValue, _maxValue, _dissolvePercentage);
            SetHeight(newValue);
            yield return null;
        }
            
        yield return null;
    }
    public void DissolveOut(float dissolveTime)
    {
        material = GetComponent<Renderer>().material;
        _dissolvePercentage = 1;
        _startTime = Time.time;
        StartCoroutine(DissolveOutRoutine(dissolveTime));

    }
    private IEnumerator DissolveOutRoutine(float dissolveTime)
    {
        while (_dissolvePercentage > 0f)
        {    
            _dissolvePercentage = (((Time.time - _startTime) / dissolveTime) - 1) * -1;            
            float newValue = Mathf.Lerp(_minValue, _maxValue, _dissolvePercentage);    
            SetHeight(newValue);
            yield return null;
        }
    }
}
