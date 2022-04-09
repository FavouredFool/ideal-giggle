using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ReferenceHelper;
using static ViewHelper;
using static EntityHelper;
using static CheckHelper;

public abstract class ReferenceController2D : AbstractReferenceController
{
    protected AbstractEntityController _thisEntity;


    public List<EntityReference> CalculateReferences2D(List<AbstractEntityController> entityList, Vector3 position)
    {
        _entityCache = entityList;
        _entityReferences = new List<EntityReference> { null, null };
        _position = position;
        _thisEntity = GetComponent<AbstractEntityController>();

        if (!entityList.Contains(_thisEntity))
        {
            return _entityReferences;
        }

        if (EntityExistsInList(_entityCache, _position + Vector3.up))
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

        if (ActiveViewState.Equals(ViewState.NX) || ActiveViewState.Equals(ViewState.X))
        {
            if (index == 0)
            {
                localReferenceDirection = Vector3.forward;
            }
            else
            {
                localReferenceDirection = Vector3.back;
            }
        }
        else
        {
            if (index == 0)
            {
                localReferenceDirection = Vector3.right;
            }
            else
            {
                localReferenceDirection = Vector3.left;
            }
        }


        return localReferenceDirection;
    }

    protected abstract void EvaluateUpperRow2D(int index);

    protected abstract void EvaluateMiddleRow2D(int index);

    protected abstract void EvaluateLowerRow2D(int index);
}

