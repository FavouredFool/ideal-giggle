using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class PlayerVisualController : MonoBehaviour
{
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

        StartCoroutine(PlayCubeBehaviour(startPosition));
    }


    public IEnumerator PlayCubeBehaviour(Vector3 fromPosition)
    {
        //Vector3 fromPosition = _playerMovement.transform.position;
        Vector3 toPosition = _playerMovement.transform.position;

        yield return _step.GetCubeBehavior().MoveCubeVisual(fromPosition, toPosition);
        EndAnimation();
    }


    public void EndAnimation()
    {
        _playerMovement.SetIsMoving(false);
    }
}
