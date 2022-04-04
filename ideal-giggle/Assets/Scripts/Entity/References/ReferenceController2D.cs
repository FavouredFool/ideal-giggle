using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceHelper;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;

public class ReferenceController2D : AbstractReferenceController
{

    [Header("Dependencies")]

    [SerializeField]
    AbstractEntityController _thisEntity;


    public List<EntityReference> CalculateReferences2D(List<AbstractEntityController> entityList, Vector3 position)
    {
        _entityCache = entityList;
        _entityReferences = new List<EntityReference> { null, null };
        _position = position;

        if (!entityList.Contains(GetComponent<AbstractEntityController>()))
        {
            return _entityReferences;
        }

        for (int i = 0; i < 2; i++)
        {
            _referenceDirection = SwitchReferenceDirection2D(i);
            _transitionIsSet = false;

            EvaluateUpperRow2D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateMiddleRow2D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateLowerRow2D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            _entityReferences[i] = null;
        }

        return _entityReferences;
    }

    public Vector3 SwitchReferenceDirection2D(int index)
    {
        Vector3 localReferenceDirection;

        switch (index)
        {
            case 0:
                if (ViewDimension.Dimension.Equals(Dimension.TWO_NX) || ViewDimension.Dimension.Equals(Dimension.TWO_X))
                {
                    localReferenceDirection = Vector3.forward;
                }
                else
                {
                    localReferenceDirection = Vector3.right;
                }
                break;
            case 1:
                if (ViewDimension.Dimension.Equals(Dimension.TWO_NX) || ViewDimension.Dimension.Equals(Dimension.TWO_X))
                {
                    localReferenceDirection = Vector3.back;
                }
                else
                {
                    localReferenceDirection = Vector3.left;
                }
                break;
            default:
                localReferenceDirection = Vector3.zero;
                Debug.LogWarning($"FEHLER: referenceDirection darf niemals {localReferenceDirection} sein.");
                break;
        }

        return localReferenceDirection;
    }

    protected void EvaluateUpperRow2D(int index)
    {
        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.up);

        if (!entity)
        {
            return;
        }

        switch (_thisEntity.GetEntityType2D())
        {
            case EntityType.BLOCK:

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
                break;

            case EntityType.STAIR:

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
                break;

            case EntityType.NONE:
                Debug.LogWarning("FEHLER");
                break;
        }
    }

    protected void EvaluateMiddleRow2D(int index)
    {

        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection);

        if (!entity)
        {
            return;
        }

        switch (_thisEntity.GetEntityType2D())
        {
            case EntityType.BLOCK:

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

                        SetReference(index, entity, ReferenceBehaviourType.EVEN);
                        break;

                    case EntityType.STAIR:
                        StairController stairEntity = (StairController)entity;

                        if (EntityExistsInList(_entityCache, _position + Vector3.up))
                        {
                            return;
                        }
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

                    default:
                        Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                        break;
                }


                break;


            case EntityType.STAIR:

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

                    case EntityType.STAIR:
                        break;
                    default:
                        Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                        break;
                }

                break;


            case EntityType.NONE:
                Debug.LogWarning("FEHLER");
                break;
        }



    }

    protected void EvaluateLowerRow2D(int index)
    {

        AbstractEntityController entity = GetEntityInListFromPos(_entityCache, _position + _referenceDirection + Vector3.down);

        if (!entity)
        {
            return;
        }

        switch (_thisEntity.GetEntityType2D())
        {
            case EntityType.STAIR:

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

                    default:
                        Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {entity.GetEntityType()} sein");
                        break;
                }


                break;


            case EntityType.NONE:
                Debug.LogWarning("FEHLER");
                break;
        }

    }
}
