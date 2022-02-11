using System.Collections;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlayerMovementController _playerMovement;

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

        StartCoroutine(PlayCubeBehaviour(startPosition, endPosition));
    }


    public IEnumerator PlayCubeBehaviour(Vector3 startPosition, Vector3 endPosition)
    {
        yield return _step.GetCubeBehavior().MoveCubeVisual(startPosition, endPosition);
        EndAnimation();
    }


    public void EndAnimation()
    {
        _playerMovement.SetIsMoving(false);
    }
}
