using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class CubeBehaviourCalculator : MonoBehaviour
{

    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerVisualController _playerVisual;

    public AbstractCubeBehaviour CalculateCubeBehaviour(AbstractEntityController activeEntity, AbstractEntityController stepGoalEntity)
    {
        EntityType entityTypeFrom = activeEntity.GetEntityType();
        EntityType entityTypeTo = stepGoalEntity.GetEntityType();

        AbstractCubeBehaviour cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToBlock>();
        bool isReverted = false;

        switch (entityTypeFrom)
        {
            case EntityType.BLOCK:

                switch (entityTypeTo)
                {
                    case (EntityType.BLOCK):
                        cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToBlock>();
                        break;
                    case (EntityType.STAIR):
                        cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToStair>();

                        isReverted = activeEntity.GetPosition().y < stepGoalEntity.GetPosition().y;
                        if (isReverted)
                        {
                            cubeBehaviour.SetIsRevered(true);
                        } else
                        {
                            cubeBehaviour.SetIsRevered(false);
                        }

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
                        cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToBlock>();

                        isReverted = activeEntity.GetPosition().y > stepGoalEntity.GetPosition().y;
                        if (isReverted)
                        {
                            cubeBehaviour.SetIsRevered(true);
                        }
                        else
                        {
                            cubeBehaviour.SetIsRevered(false);
                        }
                        break;

                    case (EntityType.STAIR):
                        if (activeEntity.GetPosition().y == stepGoalEntity.GetPosition().y)
                        {
                            cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairEven>();
                        } else
                        {
                            cubeBehaviour = _playerVisual.GetComponent <CubeBehaviour_StairToStairUneven>();

                            isReverted = activeEntity.GetPosition().y > stepGoalEntity.GetPosition().y;
                            if (isReverted)
                            {
                                cubeBehaviour.SetIsRevered(true);
                            }
                            else
                            {
                                cubeBehaviour.SetIsRevered(false);
                            }
                        }

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

        return cubeBehaviour;
    }


}
