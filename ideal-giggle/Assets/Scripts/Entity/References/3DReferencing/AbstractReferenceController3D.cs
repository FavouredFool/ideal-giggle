using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static ReferenceHelper;

public abstract class AbstractReferenceController3D : AbstractReferenceController
{

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


    protected abstract void EvaluateUpperRow3D(int index);

    protected abstract void EvaluateMiddleRow3D(int index);

    protected abstract void EvaluateLowerRow3D(int index);
    
}
