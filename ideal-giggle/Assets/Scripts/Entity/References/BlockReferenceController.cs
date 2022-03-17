using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class BlockReferenceController : AbstractReferenceController
{

    protected override void EvaluateUpperRow3D(int index)
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

    protected override void EvaluateMiddleRow3D(int index)
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

    protected override void EvaluateLowerRow3D(int index)
    {
        return;
    }

    protected override void EvaluateUpperRow2D(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            bool horizontalGuard = activeEntity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + _referenceDirection[_posWidthIndex] + Vector3.up[_posWidthIndex]);
            bool verticalGuard = activeEntity.GetPosition()[1].Equals(_position[1] + _referenceDirection[1] + Vector3.up[1]);

            if (!(horizontalGuard && verticalGuard))
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
                        bool horizontalCeilingGuard = ceilingEntity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + Vector3.up[_posWidthIndex] * 2);
                        bool verticalCeilingGuard = ceilingEntity.GetPosition()[1].Equals(_position[1] + Vector3.up[1] * 2);

                        if (horizontalCeilingGuard && verticalCeilingGuard)
                        {
                            foundCeil = true;
                            break;
                        }

                        horizontalCeilingGuard = ceilingEntity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + _referenceDirection[_posWidthIndex] + Vector3.up[_posWidthIndex] * 2);
                        verticalCeilingGuard = ceilingEntity.GetPosition()[1].Equals(_position[1] + _referenceDirection[1] + Vector3.up[1] * 2);


                        if (horizontalCeilingGuard && verticalCeilingGuard)
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

    protected override void EvaluateMiddleRow2D(int index)
    {
        
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            bool horizontalGuard = activeEntity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + _referenceDirection[_posWidthIndex]);
            bool verticalGuard = activeEntity.GetPosition()[1].Equals(_position[1] + _referenceDirection[1]);

            if (!(horizontalGuard && verticalGuard))
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
    protected override void EvaluateLowerRow2D(int index)
    {
        return;
    }




}