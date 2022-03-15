using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public abstract class AbstractReferenceController : MonoBehaviour
{
    protected List<AbstractEntityController> _entityReferences;
    protected List<AbstractEntityController> _entityCache;
    protected Vector3 _position;
    protected bool _transitionIsSet = false;
    protected Vector3 _referenceDirection;
    protected Dimension _dimension;


    public List<AbstractEntityController> CalculateReferences3D(List<AbstractEntityController> entityCache, Vector3 position)
    {
        _entityCache = entityCache;
        _entityReferences = new List<AbstractEntityController> { null, null, null, null };
        _dimension = Dimension.THREE;
        _position = position;

        for (int i = 0; i < _entityReferences.Count; i++)
        {
            _entityReferences[i] = null;
        }

        for (int i = 0; i < 4; i++)
        {
            _referenceDirection = SwitchReferenceDirection(i);
            _transitionIsSet = false;

            EvaluateUpperRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateMiddleRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateLowerRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            _entityReferences[i] = null;
        }

        return _entityReferences;
    }

    public List<AbstractEntityController> CalculateReferences2D(List<AbstractEntityController> entityCache, Dimension dimension, Vector3 position, PlaneController xPlane, PlaneController zPlane)
    {
        _entityCache = entityCache;
        _entityReferences = new List<AbstractEntityController> { null, null, null, null };
        _dimension = dimension;
        _position = position;

        return null;
    }

    public Vector3 SwitchReferenceDirection(int index)
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

    protected abstract void EvaluateUpperRow(int index);

    protected abstract void EvaluateMiddleRow(int index);

    protected abstract void EvaluateLowerRow(int index);
}
