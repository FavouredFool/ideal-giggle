using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;

public class StairReferenceController : AbstractReferenceController
{
    StairController _thisStairEntity;

    void Awake()
    {
        _thisStairEntity = GetComponent<StairController>();
    }

    protected override void EvaluateUpperRow3D(int index)
    {
        AbstractEntityController entity = _entityCache.Where(entity => EntityCheck(entity, _referenceDirection + Vector3.up)).FirstOrDefault();

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
            {
            case EntityType.STAIR:

                if (StairRotationGuard(_thisStairEntity.GetBottomEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetBottomEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                Vector3 abovePos = Vector3.up * 2;
                Vector3 onStairsPos = _referenceDirection + Vector3.up * 2;

                if (_entityCache.Any(e => EntityCheck(e, abovePos) || EntityCheck(e, onStairsPos)))
                {
                    SetReference(index, null);
                    break;
                }

                SetReference(index, entity);
                break;

            default:
                SetReference(index, null);
                break;
            }
    }

    protected override void EvaluateMiddleRow3D(int index)
    {
        AbstractEntityController entity = _entityCache.Where(entity => EntityCheck(entity, _referenceDirection)).FirstOrDefault();

        if (!entity)
        {
            return;
        }

            switch (entity.GetEntityType())
            {
                case EntityType.BLOCK:

                if (StairRotationGuard(_thisStairEntity.GetBottomEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                SetReference(index, entity);
                break;

                case EntityType.STAIR:

                if (StairRotationGuardNegated(_thisStairEntity.GetTopEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                StairController stairEntity = (StairController)entity;
                if (StairRotationGuardNegated(stairEntity.GetBottomEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                SetReference(index, entity);
                break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                SetReference(index, null);
                break;
            }

    }

    protected override void EvaluateLowerRow3D(int index)
    {
        AbstractEntityController entity = _entityCache.Where(entity => EntityCheck(entity, _referenceDirection + Vector3.down)).FirstOrDefault();

        if (!entity)
        {
            return;
        }


            switch (entity.GetEntityType())
            {
                case EntityType.BLOCK:

                if (StairRotationGuard(_thisStairEntity.GetTopEnter()))
                {
                    SetReference(index, null);
                    break;
                }

                SetReference(index, entity);
                    break;

                case EntityType.STAIR:

                if (StairRotationGuard(_thisStairEntity.GetTopEnter()))
                {
                    SetReference(index, null);
                    break;
                }
                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetTopEnter()))
                {
                    SetReference(index, null);
                    break;
                }
                SetReference(index, entity);
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                SetReference(index, null);
                break;
            }

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

                    if (!_referenceDirection.Equals(-_thisStairEntity.GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

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

                    if (!_referenceDirection.Equals(-_thisStairEntity.GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:
                    if (!_referenceDirection.Equals(-_thisStairEntity.GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

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
        foreach (AbstractEntityController activeEntity in _entityCache)
        {

            bool horizontalGuard = activeEntity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + _referenceDirection[_posWidthIndex] + Vector3.down[_posWidthIndex]);
            bool verticalGuard = activeEntity.GetPosition()[1].Equals(_position[1] + _referenceDirection[1] + Vector3.down[1]);

            if (!(horizontalGuard && verticalGuard))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:

                    if (!_referenceDirection.Equals(-_thisStairEntity.GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(-_thisStairEntity.GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

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

    

}