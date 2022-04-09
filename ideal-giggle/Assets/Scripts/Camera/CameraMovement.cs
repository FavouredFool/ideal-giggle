using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;
using static PlaneHelper;
using static TWODHelper;


public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerMovementController _playerMovementController;

    [Header("Camera Configurations")]
    [SerializeField]
    private ViewState _initialViewState;

    [SerializeField]
    private bool[] _lockedHorizontalState;


    private Camera _camera;
    private Vector3 _pivot;
    private CameraAnimation _cameraAnimation;


    private void Start()
    {
        _cameraAnimation = GetComponent<CameraAnimation>();
        _camera = GetComponentInChildren<Camera>();

        ActiveViewState = _initialViewState;

        _pivot = _entityManager.GetPivot();

        SetCamera();
        
    }

    public void SetCamera()
    {
        Vector3 cameraPosition;
        cameraPosition = _pivot + GetViewDirection() * 20;

        Quaternion cameraRotation;
        cameraRotation = Quaternion.LookRotation(-GetViewDirection());

        transform.position = cameraPosition;
        transform.rotation = cameraRotation;


        // Calculate CameraSize:
        Vector3 levelSize = _entityManager.GetLevelSize();
        float size = Mathf.Max(Mathf.Max(levelSize.x, levelSize.y), levelSize.z);

        _camera.orthographicSize = size / 2 + 4;


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
        
        UpdateView(desiredViewState);
    }

    public int CalculateVerticalDegreesRelativeToViewState(ViewState desiredState)
    {
        bool viewStateEven = (int) ActiveViewState % 2 == 0;
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
        int tempState = (int)ActiveViewState;
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
                tempState += directionSign;
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
        int tempState = (int)ActiveViewState;
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
            moveSign = +1;
        }
        else
        {
            moveSign = -1;
        }

        tempState = CalculateTempState(tempState, moveSign * moveAmount);

        return (ViewState)tempState;

    }    

    void UpdateView(ViewState desiredViewState)
    {
        // Wenn aus 2D heraus geswapped wird
        if (!ActiveViewStateIsThreeD())
        {
            _playerMovementController.MovePlayerToFront(true);
        }

        ActiveViewState = desiredViewState;
        _entityManager.UpdateReferences();

        // Wenn in 2D herein geswapped wurde
        if (!ActiveViewStateIsThreeD())
        {
            _playerMovementController.MovePlayerToFront(false);
        }

    }

    public bool InputValid()
    {
        if (!ActiveViewStateIsThreeD())
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
