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

    private bool _isRotating;

    public void Awake()
    {
        ActiveViewState = _initialViewState;
    }

    private void Start()
    {
        _cameraAnimation = GetComponent<CameraAnimation>();
        _camera = GetComponentInChildren<Camera>();

        _pivot = _entityManager.GetPivot();

        SetCamera();
        
    }

    public void SetCamera()
    {
        Vector3 cameraPosition;
        cameraPosition = _pivot - GetViewDirectionNormalized(ActiveViewState) * 20;

        Quaternion cameraRotation;
        cameraRotation = Quaternion.LookRotation(GetViewDirectionNormalized(ActiveViewState));

        transform.position = cameraPosition;
        transform.rotation = cameraRotation;


        // Calculate CameraSize:
        Vector3 levelSize = _entityManager.GetLevelSize();
        float size = Mathf.Max(Mathf.Max(levelSize.x, levelSize.y), levelSize.z);

        _camera.orthographicSize = size / 2 + 4;
    }

    public void InterpretInput(HorizontalDirection horizontalDirection, VerticalDirection verticalDirection)
    {
        if (!_entityManager.InputValid())
        {
            return;
        }

        if (_isRotating)
        {
            return;
        }

        ViewState desiredViewState = CalculateViewStateFromDirections(horizontalDirection, verticalDirection);

        _isRotating = true;
        StartCoroutine(AnimateCamera(desiredViewState));
    }

    public IEnumerator AnimateCamera(ViewState desiredViewState)
    {
        yield return _cameraAnimation.AnimateCamera(_pivot, desiredViewState);

        _isRotating = false;
        OnAnimationEnd(desiredViewState);
    }

    public void OnAnimationEnd(ViewState desiredViewState)
    {
        _entityManager.UpdateReferencePipeline(desiredViewState);
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

}
