using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class CameraInputKeyboard : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private CameraMovement _camera;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _camera.InterpretInput(HorizontalDirection.LEFT, VerticalDirection.LOWER);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _camera.InterpretInput(HorizontalDirection.RIGHT, VerticalDirection.LOWER);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _camera.InterpretInput(HorizontalDirection.LEFT, VerticalDirection.UPPER);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _camera.InterpretInput(HorizontalDirection.RIGHT, VerticalDirection.UPPER);
        }
    }

}
