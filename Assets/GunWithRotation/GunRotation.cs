using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    [SerializeField]
    float _speed = 1.0f;


    Camera _camera;
    bool _isInstant = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        /* code snippet from https://answers.unity.com/questions/1386937/i-want-to-rotate-an-object-towards-mouse-position.html*/

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

        if (_isInstant)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            Quaternion currentRotation = transform.rotation;

            float angularDifference = Quaternion.Angle(currentRotation, targetRotation);
            // will always be positive (or zero)

            if (angularDifference > 0)
                transform.rotation = Quaternion.Slerp(
                                             currentRotation,
                                             targetRotation,
                                             (_speed * 180 * Time.deltaTime) / angularDifference
                                        );
            else
                transform.rotation = targetRotation;
        }

        /* code snippet from https://answers.unity.com/questions/1386937/i-want-to-rotate-an-object-towards-mouse-position.html*/
    }
}
