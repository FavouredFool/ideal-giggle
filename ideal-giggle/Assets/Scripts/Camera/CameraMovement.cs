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

    [SerializeField]
    private bool[] _lockedHorizontalState;

    private Vector3 _pivot;

    private VerticalState _verticalState;

    private HorizontalState _horizontalState;

    private void Awake()
    {
        ViewDimension.Dimension = Dimension.THREE;
    }

    private void Start()
    {
        _verticalState = VerticalState.UPPER;
        _horizontalState = HorizontalState.X_Z;

        Vector3 levelSize = _entityManager.GetLevelSize();
        _pivot = new Vector3((levelSize.x - 1) / 2f, (levelSize.y - 1) / 2f, (levelSize.z - 1) / 2f);
    }


    public void InterpretVerticalInput(VerticalState desiredVerticalState)
    {
        if (!InputValid())
        {
            return;
        }

        if (desiredVerticalState.Equals(VerticalState.UPPER) && !_verticalState.Equals(VerticalState.UPPER))
        {
            transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), -45);
            _verticalState = VerticalState.UPPER;
        }
        else if (desiredVerticalState.Equals(VerticalState.LOWER) && !_verticalState.Equals(VerticalState.LOWER))
        {
            transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), 45);
            _verticalState = VerticalState.LOWER;
        }
        else
        {
            return;
        }

        UpdateView();
    }


    public void InterpretHorizontalInput(HorizontalDirection direction)
    {
        if (!InputValid())
        {
            return;
        }

        HorizontalState desiredHorizontalState = CalculateHorizontalStateFromDirection(direction);

        int degrees = CalculateDegreesRelativeToHorizontalState(desiredHorizontalState);
        transform.RotateAround(_pivot, Vector3.up, degrees);

        _horizontalState = desiredHorizontalState;

        UpdateView();
    }



    void UpdateView()
    {
        // Wenn aus 2D heraus geswapped wird
        if (!ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            _playerMovementController.MovePlayerToFront(true);
        }

        UpdateDimension();
        _entityManager.UpdateReferences();

        // Wenn in 2D herein geswapped wurde
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
        // DAS SOLLTE HIER EIGENTLICH NICHT BLEIBEN
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

    public HorizontalState CalculateHorizontalStateFromDirection(HorizontalDirection direction)
    {
        int tempState = (int)_horizontalState;

        for (int i = 0; i < 8; i++)
        {
            if (direction.Equals(HorizontalDirection.LEFT))
            {
                tempState--;
                tempState %= 8;

                if (tempState < 0)
                {
                    tempState = 8 + tempState;
                }
            }
            else
            {
                tempState++;
                tempState %= 8;

                if (tempState < 0)
                {
                    tempState = 8 + tempState;
                }
            }

            if (_lockedHorizontalState.Length < 8)
            {
                Debug.LogWarning("FEHLER: keine korrekten LockedStates definiert");
            }

            if (!_lockedHorizontalState[tempState])
            {
                return (HorizontalState)tempState;
            }

        }

        Debug.LogWarning("FEHLER");
        return _horizontalState;
    }

    public int CalculateDegreesRelativeToHorizontalState(HorizontalState desiredHorizontalState)
    {
        int tempState = (int)_horizontalState;

        for (int i = 0; i < 8; i++)
        {
            if (tempState != (int)desiredHorizontalState)
            {
                tempState++;
                tempState %= 8;
            }
            else
            {
                return i * -45;
            }
        }

        Debug.LogWarning("FEHLER");
        return 0;
    }
}
