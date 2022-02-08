using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController _playerMovement;

    private Vector3 _startLocalPosition;

    private Step _step;

    public void MoveStep(Step step)
    {
        _startLocalPosition = transform.localPosition;
        _step = step;

        StartCoroutine(PlayCubeBehaviour());
    }


    public IEnumerator PlayCubeBehaviour()
    {
        Vector3 fromPosition = _playerMovement.transform.position;
        Vector3 toPosition = _step.GetStepGoal();

        yield return _step.GetCubeBehavior().MoveCubeVisual(fromPosition, toPosition);
        PlacePlayer();
    }


    public void PlacePlayer()
    {
        _playerMovement.transform.position = _step.GetStepGoal();
        transform.localPosition = _startLocalPosition;

        _playerMovement.SetIsMoving(false);
    }
}
