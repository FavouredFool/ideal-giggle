using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class CameraInputController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private CameraMovement _camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _camera.MoveCameraVertically(VerticalState.LOWER);
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            _camera.MoveCameraVertically(VerticalState.UPPER);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _camera.MoveCameraHorizontally(45);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _camera.MoveCameraHorizontally(-45);
        }
    }



}
