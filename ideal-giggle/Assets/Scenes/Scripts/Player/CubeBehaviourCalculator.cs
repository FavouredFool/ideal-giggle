using UnityEngine;
using static EntityHelper;

public class CubeBehaviourCalculator : MonoBehaviour
{
    [SerializeField]
    private PlayerVisualController _playerVisual;

    private AbstractEntityController _activeEntity;
    private AbstractEntityController _goalEntity;

    private EntityType _activeEntityType;
    private EntityType _goalEntityType;

    private bool _isReversed;
    AbstractCubeBehaviour _cubeBehaviour = null;

    public AbstractCubeBehaviour CalculateCubeBehaviour(AbstractEntityController activeEntity, AbstractEntityController stepGoalEntity)
    {
        _activeEntity = activeEntity;
        _goalEntity = stepGoalEntity;

        _activeEntityType = _activeEntity.GetEntityType();
        _goalEntityType = _goalEntity.GetEntityType();

        SwitchEntityType();

        return _cubeBehaviour;
    }

    private void SwitchEntityType()
    {
        switch (_activeEntityType)
        {
            case EntityType.BLOCK:
                SwitchBlockCase();
                break;

            case EntityType.STAIR:
                SwitchStairCase();
                break;

            default:
                Debug.LogWarning($"FEHLER: entityTypeFrom ist: {_activeEntityType}");
                break;
        }
    }

    private void SwitchBlockCase()
    {
        switch (_goalEntityType)
        {
            case (EntityType.BLOCK):
                _cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToBlock>();
                break;

            case (EntityType.STAIR):
                _cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToStair>();
                if (RelativeYPos() < 0)
                {
                    _cubeBehaviour.SetIsReversed(true);
                } else
                {
                    _cubeBehaviour.SetIsReversed(false);
                }
                break;

            default:
                Debug.LogWarning($"FEHLER: entityTypeTo ist: {_goalEntityType}");
                break;
        }
    }

    private void SwitchStairCase()
    {
        switch (_goalEntityType)
        {
            case (EntityType.BLOCK):
                _cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToBlock>();

                if (RelativeYPos() <= 0)
                {
                    _cubeBehaviour.SetIsReversed(true);
                }
                else
                { 
                    _cubeBehaviour.SetIsReversed(false);
                }
                break;

            case (EntityType.STAIR):
                if (_activeEntity.GetPosition().y == _goalEntity.GetPosition().y)
                {
                    _cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairEven>();
                }
                else
                {
                    _cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_StairToStairUneven>();

                    if (RelativeYPos() < 0)
                    {
                        _cubeBehaviour.SetIsReversed(true);
                    }
                    else
                    {

                        _cubeBehaviour.SetIsReversed(false);
                    }
                }
                break;

            default:
                Debug.LogWarning($"FEHLER: entityTypeTo ist: {_goalEntityType}");
                break;
        }
    }

    private float RelativeYPos()
    {
        return _activeEntity.GetPosition().y - _goalEntity.GetPosition().y;
    }


}
