using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static ReferenceHelper;

public class BlockReferenceController : AbstractReferenceController
{

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

                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetBottomEnter()))
                {
                    break;
                }

                if (StairBlockGuard(Vector3.up*2, _referenceDirection + Vector3.up*2))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.BLOCK_UP);
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
                SetReference(index, entity, ReferenceBehaviourType.EVEN);
                break;

            case EntityType.STAIR:
                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetTopEnter()))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.BLOCK_DOWN);
                break;

            default:
                Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                break;
        }
        
    }

    protected override void EvaluateLowerRow3D(int index)
    {
        return;
    }
    protected override void EvaluateUpperRow2D(int index)
    {
        //throw new System.NotImplementedException();
    }

    protected bool EntityCheck2D(AbstractEntityController entity, Vector3 desiredDirection)
    {

        bool widthGuard = entity.GetPosition()[_posWidthIndex].Equals(_position[_posWidthIndex] + desiredDirection[_posWidthIndex]);
        //bool depthGuard = entity.GetPosition()[_posDepthIndex].Equals(_position[_posDepthIndex] + desiredDirection[_posDepthIndex]);
        bool heightGuard = entity.GetPosition().y.Equals(_position.y + desiredDirection.y);

        return widthGuard && heightGuard;
    }

    protected override void EvaluateMiddleRow2D(int index)
    {
        AbstractEntityController entity = _entityCache.Where(entity => EntityCheck2D(entity, _referenceDirection)).FirstOrDefault();

        Debug.Log($"This: {this}, entity: {entity}");

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
        {
            case EntityType.BLOCK:
                SetReference(index, entity, ReferenceBehaviourType.EVEN);
                break;

            case EntityType.STAIR:
                StairController stairEntity = (StairController)entity;
                if (StairRotationGuard(stairEntity.GetTopEnter()))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.BLOCK_DOWN);
                break;

            default:
                Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                break;
        }
    }

    protected override void EvaluateLowerRow2D(int index)
    {
        return;
    }
}