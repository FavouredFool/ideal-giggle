using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    private Vector3 pivot;

    private enum VerticalState { UPPER, LOWER };

    private VerticalState _verticalState;

    private void Start()
    {
        _verticalState = VerticalState.UPPER;

        Vector3 dimensions = _entityManager.GetDimensions();
        pivot = new Vector3((dimensions.x-1) / 2f, (dimensions.y-1) / 2f, (dimensions.z-1) / 2f);
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S) && _verticalState == VerticalState.UPPER)
        {
            _verticalState = VerticalState.LOWER;
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), 45);
        }
        else if (Input.GetKeyDown(KeyCode.W) && _verticalState == VerticalState.LOWER)
        {
            _verticalState = VerticalState.UPPER;
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), -45);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.RotateAround(pivot, Vector3.up, 45);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.RotateAround(pivot, Vector3.up, -45);
        }
    }
}
