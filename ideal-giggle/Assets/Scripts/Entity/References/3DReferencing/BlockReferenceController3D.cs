using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static ReferenceHelper;
using static CheckHelper;
using static TWODHelper;
using static ViewHelper;

public class BlockReferenceController3D : AbstractReferenceController3D
{

    protected override void EvaluateUpperRow3D(int index)
    {
        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.up);

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

                if (EntityExistsInList(_entityCache, _position + Vector3.up*2))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up * 2))
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
        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection);

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
        {
            case EntityType.BLOCK:
                if (EntityExistsInList(_entityCache,_position + _referenceDirection + Vector3.up))
                {
                    break;
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

    protected override void EvaluateLowerRow3D(int index)
    {
        return;
    }

    
}