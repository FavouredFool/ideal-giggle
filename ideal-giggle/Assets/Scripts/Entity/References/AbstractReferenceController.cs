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
    


    public List<EntityReference> CalculateReferences3D(List<AbstractEntityController> entityCache, Vector3 position)
    {
        _entityCache = entityCache;
        _entityReferences = new List<EntityReference> { null, null, null, null };
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
                if (ViewDimension.Dimension.Equals(Dimension.TWO_NX) || ViewDimension.Dimension.Equals(Dimension.TWO_X)) {
                    localReferenceDirection = Vector3.forward;
                } else
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

    protected void SetReference(int index, AbstractEntityController referencedEntity, ReferenceBehaviourType referenceBehaviour)
    {
        _entityReferences[index] = new EntityReference(referencedEntity, referenceBehaviour);
        _transitionIsSet = true;
    }

    protected abstract void EvaluateUpperRow3D(int index);

    protected abstract void EvaluateMiddleRow3D(int index);

    protected abstract void EvaluateLowerRow3D(int index);

    
    protected abstract void EvaluateUpperRow2D(int index);

    protected abstract void EvaluateMiddleRow2D(int index);

    protected abstract void EvaluateLowerRow2D(int index);
    
}
