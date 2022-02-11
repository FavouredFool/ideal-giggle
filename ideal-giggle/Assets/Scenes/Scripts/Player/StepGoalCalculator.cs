using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StepGoalCalculator : MonoBehaviour
{
    [SerializeField]
    private PlayerVisualController _playerVisual;

    [SerializeField]
    private EntityManager _entityManager;

    private Vector3 _activePosition;
    private Vector3 _endPosition;
    private Vector3 _movementVector;
    private Vector3 _stepGoal;
    private float _heightDifference;



    public AbstractEntityController CalculateStepGoalEntity(AbstractEntityController activeEntity, List<AbstractEntityController> playerMovementPath)
    {
        AbstractEntityController endEntity = null;

        for (int i = 0; i < playerMovementPath.Count; i++)
        {
            if (!playerMovementPath[i].Equals(activeEntity))
            {
                continue;
            }

            if (playerMovementPath[i+1].Equals(null)) {
                Debug.LogWarning($"FEHLER: Das gesuchte Path-Item ist null");
                break;
            }

            endEntity = playerMovementPath[i + 1];
        }


        return endEntity;
    }

    public void FromCubeBehaviour()
    {

        AbstractEntityController entityFrom = _entityManager.GetEntityFromCoordiantes(_activePosition + (Vector3.down));
        AbstractEntityController entityTo = _entityManager.GetEntityFromCoordiantes(_stepGoal + (Vector3.down));

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
