using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;


public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerMovementController _playerMovementController;

    private Vector3 pivot;

    private VerticalState _verticalState;

    private void Awake()
    {
        ViewDimension.Dimension = Dimension.THREE;
    }

    private void Start()
    {
        _verticalState = VerticalState.UPPER;

        Vector3 levelSize = _entityManager.GetLevelSize();
        pivot = new Vector3((levelSize.x-1) / 2f, (levelSize.y-1) / 2f, (levelSize.z-1) / 2f);
    }

    private void Update()
    {
        
    }

    public void MoveCameraVertically(VerticalState desiredVerticalState)
    {
        if (desiredVerticalState.Equals(VerticalState.UPPER) && !_verticalState.Equals(VerticalState.UPPER))
        {
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), -45);
            _verticalState = VerticalState.UPPER;
        }
        else if (desiredVerticalState.Equals(VerticalState.LOWER) && !_verticalState.Equals(VerticalState.LOWER))
        {
            transform.RotateAround(pivot, Vector3.Cross(transform.forward, Vector3.up), 45);
            _verticalState = VerticalState.LOWER;
        }
        else
        {
            return;
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
        if (!ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            _playerMovementController.MovePlayerToFront(true);
        }

        UpdateDimension();
        _entityManager.UpdateReferences();

        if (!ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            _playerMovementController.MovePlayerToFront(false);
        }
        
    }

    void UpdateDimension()
    {
        if (_verticalState.Equals(VerticalState.UPPER))
        {
            ViewDimension.Dimension = Dimension.THREE;
        }
        else
        {
            if (transform.forward.V3Equal(Vector3.forward))
            {
                ViewDimension.Dimension = Dimension.TWO_NZ;
            }
            else if (transform.forward.V3Equal(Vector3.back))
            {
                ViewDimension.Dimension = Dimension.TWO_Z;
            }
            else if (transform.forward.V3Equal(Vector3.right))
            {
                ViewDimension.Dimension = Dimension.TWO_NX;
            }
            else if (transform.forward.V3Equal(Vector3.left))
            {
                ViewDimension.Dimension = Dimension.TWO_X;
            }
            else
            {
                ViewDimension.Dimension = Dimension.THREE;
            }
        }
    }

    public bool InputValid()
    {

        if (!ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            if (_playerMovementController.GetGroundEntity().GetEntityType2D().Equals(EntityType.BLOCK))
            {
                if (_playerMovementController.GetEntityPositionRelation().Equals(EntityType.BLOCK))
                {
                    if (GetEntityListFromPos2D(_entityManager.GetEntityList(), _playerMovementController.GetGroundEntity().GetPosition()).Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

}
