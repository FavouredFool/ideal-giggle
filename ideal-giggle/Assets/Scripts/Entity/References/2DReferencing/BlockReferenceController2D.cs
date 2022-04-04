using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceHelper;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;

public class BlockReferenceController2D : ReferenceController2D
{
    protected override void EvaluateUpperRow2D(int index)
    {
        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.up);

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType2D())
        {
            case EntityType.STAIR:
                StairController stairEntity = (StairController)entity;

                if (!StairRotatedInDirection(stairEntity.GetBottomEnter(), _referenceDirection))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + Vector3.up * 2))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up * 2))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.BLOCK_UP);
                break;
        }

    }


    protected override void EvaluateMiddleRow2D(int index)
    {

        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection);

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType2D())
        {
            case EntityType.BLOCK:

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up))
                {
                    return;
                }

                SetReference(index, entity, ReferenceBehaviourType.EVEN);
                break;

            case EntityType.STAIR:
                StairController stairEntity = (StairController)entity;

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up))
                {
                    return;
                }

                if (!StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.BLOCK_DOWN);
                break;
        }

    }

    protected override void EvaluateLowerRow2D(int index)
    {



    }
}



