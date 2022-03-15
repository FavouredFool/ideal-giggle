using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class BlockReferenceController : AbstractReferenceController
{

    protected override void EvaluateUpperRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!(activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.up)))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    // Check ob Treppe bedeckt ist, oder über dem Spieler ein Block ist
                    bool foundCeil = false;
                    foreach (AbstractEntityController ceilingEntity in _entityCache)
                    {
                        if (ceilingEntity.GetPosition().Equals(_position + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }

                        if (ceilingEntity.GetPosition().Equals(_position + _referenceDirection + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }
                    }

                    if (foundCeil)
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }

    protected override void EvaluateMiddleRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!activeEntity.GetPosition().Equals(_position + _referenceDirection))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:
                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {activeEntity.GetEntityType()} sein");
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }

    protected override void EvaluateLowerRow(int index)
    {
        return;
    }

    
}