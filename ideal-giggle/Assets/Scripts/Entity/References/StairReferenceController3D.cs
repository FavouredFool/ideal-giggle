using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static ReferenceHelper;
using static CheckHelper;

public class StairReferenceController3D : AbstractReferenceController3D
{
    StairController _thisStairEntity;

    void Awake()
    {
        _thisStairEntity = GetComponent<StairController>();
    }

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

                if (!StairRotatedInDirection(_thisStairEntity.GetBottomEnter(), _referenceDirection))
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

                if (!StairRotatedInDirection(_thisStairEntity.GetBottomEnter(), _referenceDirection))
                {
                    break;
                }

                SetReference(index, entity, ReferenceBehaviourType.STAIR_BLOCK_UP);
            break;

            case EntityType.STAIR:

                if (StairRotatedInDirection(_thisStairEntity.GetBottomEnter(), _referenceDirection))
                {
                    StairController stairEntity = (StairController)entity;

                    if (!StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection))
                    {
                        break;
                    }

                    SetReference(index, entity, ReferenceBehaviourType.STAIR_STAIR_EVEN);

                }
                else if (!StairRotatedInDirection(_thisStairEntity.GetTopEnter(), _referenceDirection))
                {
                    // Test ob anderer auch korrekt gedreht ist
                    StairController stairEntity = (StairController)entity;

                    if (StairRotatedInDirection(stairEntity.GetTopEnter(), _referenceDirection) || StairRotatedInDirection(stairEntity.GetBottomEnter(), _referenceDirection))
                    {
                        break;
                    }

                    SetReference(index, entity, ReferenceBehaviourType.EVEN);
                }
                else
                {
                    break;
                }
                
            break;
            default:
                Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
            break;
        }

    }

    protected override void EvaluateLowerRow3D(int index)
    {
        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.down);

        if (!entity)
        {
            return;
        }

        switch (entity.GetEntityType())
            {
                case EntityType.BLOCK:

                if (!StairRotatedInDirection(_thisStairEntity.GetTopEnter(), _referenceDirection))
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

                if (!StairRotatedInDirection(_thisStairEntity.GetTopEnter(), _referenceDirection))
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
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                break;
            }
    }

}