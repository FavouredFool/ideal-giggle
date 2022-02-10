using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController _playerMovement;

    private Vector3 _blockLocalPosition;
    private Vector3 _stairLocalPosition;

    private Step _step;

    public void MoveStep(Step step)
    {
        _blockLocalPosition = new Vector3(0, -0.25f, 0);
        _stairLocalPosition = new Vector3(0, -0.75f, 0);
        _step = step;

        Vector3 fromPosition = _playerMovement.transform.position;

        _playerMovement.transform.position = _step.GetStepGoalEntity().GetPosition() + Vector3.up;

        transform.position = fromPosition;

        StartCoroutine(PlayCubeBehaviour(fromPosition));
    }


    public IEnumerator PlayCubeBehaviour(Vector3 fromPosition)
    {
        //Vector3 fromPosition = _playerMovement.transform.position;
        Vector3 toPosition = _playerMovement.transform.position;

        yield return _step.GetCubeBehavior().MoveCubeVisual(fromPosition, toPosition);
        PlacePlayer();
    }


    public void PlacePlayer()
    {
        
        _playerMovement.SetIsMoving(false);
        _playerMovement.SetGroundEntity(_step.GetStepGoalEntity());

        if (_playerMovement.GetGroundEntity().GetEntityType().Equals(EntityType.STAIR))
        {
            transform.localPosition = _stairLocalPosition;
        } else
        {
            transform.localPosition = _blockLocalPosition;
        }
    }
}
