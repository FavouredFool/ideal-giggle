using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static ReferenceHelper;

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
                    break;
                }

                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetBottomEnter()))
                {
                    break;
                }

                if (StairBlockGuard(Vector3.up * 2, _referenceDirection + Vector3.up * 2))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_UP);
                break;

            default:
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
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_BLOCK_UP);
                break;

                case EntityType.STAIR:

                if (StairRotationGuardNegated(_thisStairEntity.GetTopEnter()))
                {
                    break;
                }

                StairController stairEntity = (StairController)entity;
                if (StairRotationGuardNegated(stairEntity.GetBottomEnter()))
                {
                    break;
                }

                if (StairRotationGuard(_thisStairEntity.GetBottomEnter()))
                {
                    SetReference(index, entity, ReferenceBehaviourType.EVEN);
                } else
                {
                    SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_EVEN);
                }
                
                break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
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
                    break;
                }

                if (StairBlockGuard(Vector3.up, _referenceDirection + Vector3.up))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_BLOCK_DOWN);
                break;

                case EntityType.STAIR:

                if (StairRotationGuard(_thisStairEntity.GetTopEnter()))
                {
                    break;
                }
                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetTopEnter()))
                {
                    break;
                }

                if (StairBlockGuard(Vector3.up, _referenceDirection + Vector3.up))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_DOWN);
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                break;
            }
    }

    protected override void EvaluateUpperRow2D(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void EvaluateMiddleRow2D(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void EvaluateLowerRow2D(int index)
    {
        throw new System.NotImplementedException();
    }
}