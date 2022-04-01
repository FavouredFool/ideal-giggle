using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static ReferenceHelper;
using static CheckHelper;

public class BlockReferenceController : AbstractReferenceController
{

    protected override void EvaluateUpperRow3D(int index)
    {
        AbstractEntityController entity = EntityCheck(_referenceDirection + Vector3.up);

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
        {
            case EntityType.STAIR:

                StairController stairEntity = (StairController)entity;
                if (!StairRotatedInDirection(stairEntity.GetBottomEnter(), _referenceDirection))
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

        AbstractEntityController entity = EntityCheck(_referenceDirection);

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
                if (!StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection))
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
        bool heightGuard = entity.GetPosition().y.Equals(_position.y + desiredDirection.y);

        return widthGuard && heightGuard;
    }

    protected bool BlockGuard2D(Vector3 checkDir)
    {
        return _entityCache.Any(e => EntityCheck2D(e, checkDir));
    }

    protected override void EvaluateMiddleRow2D(int index)
    {
        AbstractEntityController entity = _entityCache.Where(entity => EntityCheck2D(entity, _referenceDirection)).FirstOrDefault();

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
        {
            case EntityType.BLOCK:

                if (BlockGuard2D(Vector3.up))
                {
                    return;
                }
                if (BlockGuard2D(Vector3.up + _referenceDirection))
                {
                    return;
                }

                SetReference(index, entity, ReferenceBehaviourType.EVEN);
                break;

            case EntityType.STAIR:
                StairController stairEntity = (StairController)entity;
                if (!StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection))
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