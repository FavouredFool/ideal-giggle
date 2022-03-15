using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private CameraMovement _camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
        {
            _camera.MoveCameraVertically();
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
