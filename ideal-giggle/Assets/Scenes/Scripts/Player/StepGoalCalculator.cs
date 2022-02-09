using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StepGoalCalculator : MonoBehaviour
{
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerVisualController _playerVisual;

    private Vector3 _activePosition;
    private Vector3 _endPosition;
    private Vector3 _movementVector;
    private Vector3 _stepGoal;
    private float _heightDifference;



    public Vector3 CalculateStepGoal(Vector3 activePosition, Vector3 endPosition)
    {
        /*
        _activePosition = activePosition;
        _endPosition = endPosition;
        _movementVector = Vector3.zero;


        int xActive = (int)_activePosition.x;
        int xEnd = (int)_endPosition.x;
        int zActive = (int)_activePosition.z;
        int zEnd = (int)_endPosition.z;

        int distX = Mathf.Abs(xActive - xEnd);
        int distZ = Mathf.Abs(zActive - zEnd);

        

        if (distX > distZ)
        {
            _movementVector.x = Mathf.Sign(xEnd - xActive);
        }
        else if (distZ > distX)
        {
            _movementVector.z = Mathf.Sign(zEnd - zActive);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                _movementVector.x = Mathf.Sign(xEnd - xActive);
            }
            else
            {
                _movementVector.z = Mathf.Sign(zEnd - zActive);
            }
        }

        _stepGoal = _activePosition + _movementVector;

        FromCubeBehaviour();

        if (_activePosition.y < _endPosition.y)
        {
            _stepGoal.y += _heightDifference;
        }
        else if (_activePosition.y > _endPosition.y)
        {
            _stepGoal.y -= _heightDifference;
        }
        */

        return endPosition;
    }

    public void FromCubeBehaviour()
    {
        Debug.Log($"ActivePosition: {_activePosition}");
        Debug.Log($"StepGoal: {_stepGoal}");

        EntityController entityFrom = _entityManager.GetEntityFromCoordiantes(_activePosition + (Vector3.down));
        EntityController entityTo = _entityManager.GetEntityFromCoordiantes(_stepGoal + (Vector3.down));

        Debug.Log($"Entity: {entityTo}");
        foreach (EntityController activeReference in entityTo.GetEntityReferences())
        {
            Debug.Log($"EntityReference: " + activeReference);
        }
        Debug.Log("\n");
        

        EntityType entityTypeFrom = EntityType.NONE;
        EntityType entityTypeTo = EntityType.NONE;


        if (entityFrom)
        {
            entityTypeFrom = entityFrom.GetEntityType();
        }
        if (entityTo)
        {
            entityTypeTo = entityTo.GetEntityType();
        }
        _heightDifference = 0;

        switch (entityTypeFrom)
        {
            case EntityType.BLOCK:

                switch (entityTypeTo)
                {
                    case (EntityType.BLOCK):
                        _heightDifference = 0f;
                        break;
                    case (EntityType.STAIR):
                        _heightDifference = 0;
                        break;
                    default:
                        Debug.LogWarning($"FEHLER: entityTypeTo ist: {entityTypeTo}");
                        break;
                }
                break;

            case EntityType.STAIR:

                switch (entityTypeTo)
                {
                    case (EntityType.BLOCK):
                        _heightDifference = 0f;
                        break;
                    case (EntityType.STAIR):
                        _heightDifference = 1f;
                        break;
                    default:
                        Debug.LogWarning($"FEHLER: entityTypeTo ist: {entityTypeTo}");
                        break;
                }
                break;

            default:
                Debug.LogWarning($"FEHLER: entityTypeFrom ist: {entityTypeFrom}");
                break;
        }

    }


}
