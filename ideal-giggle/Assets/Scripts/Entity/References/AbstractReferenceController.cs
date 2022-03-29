using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static ReferenceHelper;

public abstract class AbstractReferenceController : MonoBehaviour
{
    protected List<EntityReference> _entityReferences;
    protected List<AbstractEntityController> _entityCache;
    protected Vector3 _position;
    protected bool _transitionIsSet = false;
    protected Vector3 _referenceDirection;
    protected Dimension _dimension;
    protected int _posDepthIndex;
    protected int _posWidthIndex;
    


    public List<EntityReference> CalculateReferences3D(List<AbstractEntityController> entityCache, Vector3 position)
    {
        _entityCache = entityCache;
        _entityReferences = new List<EntityReference> { null, null, null, null };
        _dimension = Dimension.THREE;
        _position = position;

        for (int i = 0; i < 4; i++)
        {
            _referenceDirection = SwitchReferenceDirection3D(i);
            _transitionIsSet = false;

            EvaluateUpperRow3D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateMiddleRow3D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateLowerRow3D(i);
            if (_transitionIsSet)
            {
                continue;
            }

            _entityReferences[i] = null;
        }

        return _entityReferences;
    }

    /*
    public List<EntityReference> CalculateReferences2D(List<AbstractEntityController> entityCache, Dimension dimension, Vector3 position)
    {
        _entityCache = entityCache;
        _entityReferences = new List<EntityReference> { null, null };
        _dimension = dimension;
        _position = position;
        
        switch (dimension)
        {
            case Dimension.TWO_X:
                _posDepthIndex = 0;
                _posWidthIndex = 2;
                break;
            case Dimension.TWO_NX:
                _posDepthIndex = 0;
                _posWidthIndex = 2;
                break;
            case Dimension.TWO_Z:
                _posDepthIndex = 2;
                _posWidthIndex = 0;
                break;
            case Dimension.TWO_NZ:
                _posDepthIndex = 2;
                _posWidthIndex = 0;
                break;
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
    */

    public Vector3 SwitchReferenceDirection3D(int index)
    {
        Vector3 localReferenceDirection;

        switch (index)
        {
            case 0:
                localReferenceDirection = Vector3.forward;
                break;
            case 1:
                localReferenceDirection = Vector3.right;
                break;
            case 2:
                localReferenceDirection = Vector3.back;
                break;
            case 3:
                localReferenceDirection = Vector3.left;
                break;
            default:
                localReferenceDirection = Vector3.zero;
                Debug.LogWarning($"FEHLER: referenceDirection darf niemals {localReferenceDirection} sein.");
                break;
        }

        return localReferenceDirection;
    }

    public Vector3 SwitchReferenceDirection2D(int index)
    {
        Vector3 localReferenceDirection;

        switch (index)
        {
            case 0:
                if (_dimension.Equals(Dimension.TWO_NX) || _dimension.Equals(Dimension.TWO_X)) {
                    localReferenceDirection = Vector3.forward;
                } else
                {
                    localReferenceDirection = Vector3.right;
                }
                break;
            case 1:
                if (_dimension.Equals(Dimension.TWO_NX) || _dimension.Equals(Dimension.TWO_X))
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

    protected void SetReference(int index, AbstractEntityController referencedEntity, ReferenceBehaviourType referenceBehaviour)
    {
        _entityReferences[index] = new EntityReference(referencedEntity, referenceBehaviour);
        _transitionIsSet = true;
    }


    protected bool StairRotationGuard(Vector3 stairEnter)
    {
        return !_referenceDirection.Equals(stairEnter);
    }

    protected bool StairRotationGuardNegated(Vector3 stairEnter)
    {
        return _referenceDirection.Equals(stairEnter);
    }

    protected bool EntityCheck(AbstractEntityController entity, Vector3 desiredDirection)
    {
        return entity.GetPosition().Equals(_position + desiredDirection);
    }

    protected bool StairBlockGuard(Vector3 checkPos1, Vector3 checkPos2)
    {
        return _entityCache.Any(e => EntityCheck(e, checkPos1) || EntityCheck(e, checkPos2));
    }

    protected abstract void EvaluateUpperRow3D(int index);

    protected abstract void EvaluateMiddleRow3D(int index);

    protected abstract void EvaluateLowerRow3D(int index);

    /*
    protected abstract void EvaluateUpperRow2D(int index);

    protected abstract void EvaluateMiddleRow2D(int index);

    protected abstract void EvaluateLowerRow2D(int index);
    */
}
