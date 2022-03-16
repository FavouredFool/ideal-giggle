using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    private Vector3 pivot;

    private VerticalState _verticalState;
    private Dimension _dimension;

    private void Start()
    {
        _verticalState = VerticalState.UPPER;
        _dimension = Dimension.THREE;

        Vector3 dimensions = _entityManager.GetLevelSize();
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

        UpdateView();
    }


    public void MoveCameraHorizontally(int degrees)
    {
        transform.RotateAround(pivot, Vector3.up, degrees);

        UpdateView();
    }

    void UpdateView()
    {
        UpdateDimension();

        _entityManager.UpdateReferences(_dimension);
    }

    void UpdateDimension()
    {
        if (_verticalState.Equals(VerticalState.UPPER))
        {
            _dimension = Dimension.THREE;
        }
        else
        {
            if (transform.forward.V3Equal(Vector3.forward))
            {
                _dimension = Dimension.TWO_NZ;
            }
            else if (transform.forward.V3Equal(Vector3.back))
            {
                _dimension = Dimension.TWO_Z;
            }
            else if (transform.forward.V3Equal(Vector3.right))
            {
                _dimension = Dimension.TWO_NX;
            }
            else if (transform.forward.V3Equal(Vector3.left))
            {
                _dimension = Dimension.TWO_X;
            }
            else
            {
                _dimension = Dimension.THREE;
            }
        }
    }

}
