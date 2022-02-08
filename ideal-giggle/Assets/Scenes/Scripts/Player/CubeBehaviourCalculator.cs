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

    public ICubeBehaviour CalculateCubeBehaviour(Vector3 activePosition, Vector3 stepGoal)
    {
        EntityController entityFrom = _entityManager.GetEntityFromCoordiantes(activePosition + Vector3.down);
        EntityController entityTo = _entityManager.GetEntityFromCoordiantes(stepGoal + Vector3.down);

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
        

        ICubeBehaviour cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToBlock>();

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
                        break;
                    case (EntityType.STAIR):
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
