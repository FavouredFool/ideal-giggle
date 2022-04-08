using System.Collections;
using UnityEngine;
using static ViewHelper;

public class PlayerVisualController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlayerMovementController _playerMovement;

    [SerializeField]
    private CameraMovement _cameraMovement;

    private Step _step;

    public void MoveStep(Step step)
    {
        _step = step;

        AbstractEntityController startEntity = _playerMovement.GetGroundEntity();
        AbstractEntityController goalEntity = _step.GetStepGoalEntity();

        Vector3 startPosition = startEntity.GetAdjacentPosition(Vector3.up);
        Vector3 endPosition = goalEntity.GetAdjacentPosition(Vector3.up);

        _playerMovement.transform.localPosition = endPosition;
        _playerMovement.SetGroundEntity(goalEntity);
        transform.position = startPosition + startEntity.GetVisualPosition();

        if (ActiveViewStateIsThreeD())
        {
            _playerMovement.SetEntityPositionRelation(_playerMovement.GetGroundEntity().GetEntityType());
        }
        else
        {
            _playerMovement.SetEntityPositionRelation(_playerMovement.GetGroundEntity().GetEntityType2D());
        }

        StartCoroutine(PlayCubeBehaviour(startPosition, endPosition));
    }


    public IEnumerator PlayCubeBehaviour(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = Vector3.zero;
        int xDirection = (int)endPosition.x - (int)startPosition.x;
        int zDirection = (int)endPosition.z - (int)startPosition.z;

        switch (ActiveViewState)
        {
            case ViewState.X:
                direction = Vector3.forward * Mathf.Sign((int)endPosition.z - (int)startPosition.z);
                break;
            case ViewState.NX:
                direction = Vector3.forward * Mathf.Sign((int)endPosition.z - (int)startPosition.z);
                break;
            case ViewState.Z:
                direction = Vector3.right * Mathf.Sign((int)endPosition.x - (int)startPosition.x);
                break;
            case ViewState.NZ:
                direction = Vector3.right * Mathf.Sign((int)endPosition.x - (int)startPosition.x);
                break;
            default:
                direction = new Vector3(xDirection, 0, zDirection).normalized;
                break;
        }

        yield return _step.GetCubeBehavior().MoveCubeVisual(direction);
        EndAnimation();
    }


    public void EndAnimation()
    {
        transform.position = _playerMovement.GetGroundEntity().GetAdjacentPosition(Vector3.up) + _playerMovement.GetGroundEntity().GetVisualPosition();
        _playerMovement.SetIsMoving(false);
    }
}
