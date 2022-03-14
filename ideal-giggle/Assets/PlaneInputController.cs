using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInputController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlaneController _xPlane;

    [SerializeField]
    private PlaneController _zPlane;

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _zPlane.MovePlane(Vector3.back);
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            _zPlane.MovePlane(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _xPlane.MovePlane(Vector3.left);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            _xPlane.MovePlane(Vector3.right);
        }
    }
}
