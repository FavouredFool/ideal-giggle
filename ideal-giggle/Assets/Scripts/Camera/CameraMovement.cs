using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;
using static PlaneHelper;


public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerMovementController _playerMovementController;

    [SerializeField]
    private bool[] _lockedHorizontalState;

    [SerializeField]
    private ViewState _initialViewState;

    private ViewState _viewState;

    private Vector3 _pivot;

    private CameraAnimation _cameraAnimation;

    

    private void Awake()
    {
        ViewDimension.Dimension = Dimension.TWO_NZ;
    }

    private void Start()
    {
        _cameraAnimation = GetComponent<CameraAnimation>();
        _viewState = _initialViewState;

        _pivot = _entityManager.GetPivot();

        CalculateCameraPosition();
        CalculateInitialViewState();
    }

    public void CalculateCameraPosition()
    {

    }

    public void CalculateInitialViewState()
    {

    }

    public void InterpretInput(HorizontalDirection horizontalDirection, VerticalDirection verticalDirection)
    {
        if (!InputValid())
        {
            return;
        }

        ViewState desiredViewState = CalculateViewStateFromDirections(horizontalDirection, verticalDirection);

        int hDegrees = CalculateHorizontalDegreesRelativeToViewState(desiredViewState, horizontalDirection);
        int vDegrees = CalculateVerticalDegreesRelativeToViewState(desiredViewState);

        StartCoroutine(AnimateCamera(desiredViewState, hDegrees, vDegrees));
    }

    public IEnumerator AnimateCamera(ViewState desiredViewState, int hDegrees, int vDegrees)
    {
        yield return _cameraAnimation.AnimateCamera(_pivot, hDegrees, vDegrees, desiredViewState);

        OnAnimationEnd(desiredViewState);
    }

    public void OnAnimationEnd(ViewState desiredViewState)
    {
        _viewState = desiredViewState;
        UpdateView();
    }

    public int CalculateVerticalDegreesRelativeToViewState(ViewState desiredState)
    {
        bool viewStateEven = (int) _viewState % 2 == 0;
        bool desiredStateEven = (int) desiredState % 2 == 0;

        if (desiredStateEven == viewStateEven)
        {
            return 0;
        }
        else if (viewStateEven)
        {
            return +45;
        }
        else
        {
            return -45;
        }
        
    }

    public int CalculateHorizontalDegreesRelativeToViewState(ViewState desiredState, HorizontalDirection horizontalDirection)
    {
        int tempState = (int)_viewState;
        int directionSign;

        if (horizontalDirection.Equals(HorizontalDirection.LEFT))
        {
            directionSign = 1;
        }
        else
        {
            directionSign = -1;
        }

        for (int i = 0; i < 8; i++)
        {
            if (tempState != (int)desiredState)
            {
                tempState += -directionSign;
                tempState = ValidateTempState(tempState);
            }
            else
            {
                return i * directionSign * 45;
            }
        }

        Debug.LogWarning("FEHLER");
        return 0;
    }

    public int CalculateTempState(int tempState, int added)
    {
        int tempStateOriginal = tempState;

        tempState += added;
        tempState = ValidateTempState(tempState);

        for (int i = 0; i < 4; i++)
        {
            if (_lockedHorizontalState.Length != 8)
            {
                Debug.LogWarning("FEHLER: keine korrekten LockedStates definiert");
            }

            if (!_lockedHorizontalState[tempState])
            {
                return tempState;
            }

            tempState += (int) Mathf.Sign(added) * 2;
            tempState = ValidateTempState(tempState);
        }

        return tempStateOriginal;
    }

    public int ValidateTempState(int tempState)
    {
        tempState %= 8;

        if (tempState < 0)
        {
            tempState = 8 + tempState;
        }

        return tempState;
    }


    public ViewState CalculateViewStateFromDirections(HorizontalDirection horizontalDirection, VerticalDirection verticalDirection)
    {
        int tempState = (int)_viewState;
        bool tempStateEven = tempState % 2 == 0;
        int moveAmount;
        int moveSign;

        if (tempStateEven != verticalDirection.Equals(VerticalDirection.UPPER))
        {
            moveAmount = 2;
        }
        else
        {
            moveAmount = 1;
        }

        if (horizontalDirection.Equals(HorizontalDirection.LEFT))
        {
            moveSign = -1;
        }
        else
        {
            moveSign = +1;
        }

        tempState = CalculateTempState(tempState, moveSign * moveAmount);

        return (ViewState)tempState;

    }

    /*
    public void InterpretVerticalInput(VerticalDirection desiredVerticalState)
    {
        if (!InputValid())
        {
            return;
        }

        if (desiredVerticalState.Equals(VerticalDirection.UPPER) && !_verticalState.Equals(VerticalDirection.UPPER))
        {
            transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), -45);
            _verticalState = VerticalDirection.UPPER;
        }
        else if (desiredVerticalState.Equals(VerticalDirection.LOWER) && !_verticalState.Equals(VerticalDirection.LOWER))
        {
            transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), 45);
            _verticalState = VerticalDirection.LOWER;
        }
        else
        {
            return;
        }

        UpdateView();
    

    }
    */
    

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

    public ViewState GetHorizontalState()
    {
        return _viewState;
    }
}
