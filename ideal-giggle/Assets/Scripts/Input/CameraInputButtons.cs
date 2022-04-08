using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class CameraInputButtons : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private CameraMovement _camera;

    public void ReceiveInputUpperLeft()
    {
        ReceiveInput(HorizontalDirection.LEFT, VerticalDirection.UPPER);
    }

    public void ReceiveInputUpperRight()
    {
        ReceiveInput(HorizontalDirection.RIGHT, VerticalDirection.UPPER);
    }

    public void ReceiveInputLowerLeft()
    {
        ReceiveInput(HorizontalDirection.LEFT, VerticalDirection.LOWER);
    }

    public void ReceiveInputLowerRight()
    {
        ReceiveInput(HorizontalDirection.RIGHT, VerticalDirection.LOWER);
    }

    public void ReceiveInput(HorizontalDirection hDirection, VerticalDirection vDirection)
    {
        _camera.InterpretInput(hDirection, vDirection);
    }
}
