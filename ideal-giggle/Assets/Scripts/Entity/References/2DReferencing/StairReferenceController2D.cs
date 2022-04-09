using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceHelper;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;

public class StairReferenceController2D : ReferenceController2D
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

                StairController thisStair = (StairController)_thisEntity;

                if (!StairRotatedInDirection(thisStair.GetBottomEnter(), _referenceDirection))
                {
                    break;
                }

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

                SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_UP);
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

        StairController thisStair = (StairController)_thisEntity;

        switch (entity.GetEntityType2D())
        {
            case EntityType.BLOCK:

                if (EntityExistsInList(_entityCache, _position + Vector3.up))
                {
                    return;
                }

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up))
                {
                    return;
                }

                if (!StairRotatedInDirection(thisStair.GetBottomEnter(), _referenceDirection))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_BLOCK_UP);
                break;
        }

    }

    protected override void EvaluateLowerRow2D(int index)
    {

        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.down);

        if (!entity)
        {
            return;
        }

        StairController thisStair = (StairController)_thisEntity;

        switch (entity.GetEntityType2D())
        {
            case EntityType.BLOCK:

                if (!StairRotatedInDirection(thisStair.GetTopEnter(), _referenceDirection))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + Vector3.up))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_BLOCK_DOWN);
                break;

            case EntityType.STAIR:

                if (!StairRotatedInDirection(thisStair.GetTopEnter(), _referenceDirection))
                {
                    break;
                }
                StairController stairEntity = (StairController)entity;
                if (!StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + Vector3.up))
                {
                    break;
                }

                if (EntityExistsInList(_entityCache, _position + _referenceDirection + Vector3.up))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_DOWN);
                break;
        }
    }

}


