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

    private enum Dimension { TWO, THREE };

    private VerticalState _verticalState;
    private Dimension _dimension;

    private void Start()
    {
        _verticalState = VerticalState.UPPER;
        _dimension = Dimension.THREE;

        Vector3 dimensions = _entityManager.GetDimensions();
        pivot = new Vector3((dimensions.x-1) / 2f, (dimensions.y-1) / 2f, (dimensions.z-1) / 2f);
    }

    public void MoveCameraVertically()
    {
       
        if (_verticalState.Equals(VerticalState.UPPER))
        {
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), 45);
            _verticalState = VerticalState.LOWER;
        }
        else if (_verticalState.Equals(VerticalState.LOWER))
        {
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), -45);
            _verticalState = VerticalState.UPPER;
        }
        else
        {
            Debug.LogWarning("FEHLER");
        }

        UpdateDimension();
        UpdateReferences();
    }

    public void MoveCameraHorizontally(int degrees)
    {
        transform.RotateAround(pivot, Vector3.up, degrees);
        UpdateDimension();
        UpdateReferences();
    }

    void UpdateDimension()
    {
        if (_verticalState.Equals(VerticalState.UPPER))
        {
            _dimension = Dimension.THREE;
        }
        else if (transform.rotation.eulerAngles.y % 90 != 0)
        {
            _dimension = Dimension.THREE;
        }
        else
        {
            _dimension = Dimension.TWO;
        }
    }

    void UpdateReferences()
    {

    }

}
