using UnityEngine;
using static EntityHelper;
using static ReferenceHelper;

public class CubeBehaviourCalculator : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private PlayerVisualController _playerVisual;

    public AbstractCubeBehaviour CalculateCubeBehaviour(AbstractEntityController activeEntity, AbstractEntityController stepGoalEntity)
    {
        AbstractCubeBehaviour cubeBehaviour = null;

        foreach (EntityReference activeEntityReference in activeEntity.GetActiveEntityReferences())
        {
            if (activeEntityReference == null)
            {
                continue;
            }

            if (activeEntityReference.GetReferenceEntity() != stepGoalEntity)
            {
                continue;
            }

            cubeBehaviour = SwitchReferenceBehaviour(activeEntityReference.GetReferenceBehaviorType());
            break;
        }
        return cubeBehaviour;
    }

    private AbstractCubeBehaviour SwitchReferenceBehaviour(ReferenceBehaviourType referenceBehaviourType)
    {
        AbstractCubeBehaviour cubeBehaviour = null;

        switch (referenceBehaviourType)
        {
            case ReferenceBehaviourType.EVEN:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_Even>();
                cubeBehaviour.SetIsReversed(false);
                break;
            case ReferenceBehaviourType.BLOCK_DOWN:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToStair>();
                cubeBehaviour.SetIsReversed(false);
                break;
            case ReferenceBehaviourType.BLOCK_UP:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToStair>();
                cubeBehaviour.SetIsReversed(true);
                break;
            case ReferenceBehaviourType.STAIR_BLOCK_DOWN:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToBlock>();
                cubeBehaviour.SetIsReversed(false);
                break;
            case ReferenceBehaviourType.STAIR_BLOCK_UP:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToBlock>();
                cubeBehaviour.SetIsReversed(true);
                break;
            case ReferenceBehaviourType.STAIR_STAIR_DOWN:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairUneven>();
                cubeBehaviour.SetIsReversed(false);
                break;
            case ReferenceBehaviourType.STAIR_STAIR_UP:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairUneven>();
                cubeBehaviour.SetIsReversed(true);
                break;
            case ReferenceBehaviourType.STAIR_STAIR_EVEN:
                cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairEven>();
                cubeBehaviour.SetIsReversed(false);
                break;
            default:
                Debug.LogWarning("FEHLER");
                break;
        }

        return cubeBehaviour;
    }


}
