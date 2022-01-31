using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseReticle : MonoBehaviour
{
    float _mouseX, _mouseY;
    [SerializeField]
    Image _reticleImage;
    [SerializeField]
    private bool _disableMouseReticle;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if(!_disableMouseReticle)
            Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_disableMouseReticle)
            TrackMouse();
    }

    private void TrackMouse()
    {
        var mousePosition = Input.mousePosition;
        _mouseX = mousePosition.x;
        _mouseY = mousePosition.y;

        _reticleImage.rectTransform.position = mousePosition;
    }
}
