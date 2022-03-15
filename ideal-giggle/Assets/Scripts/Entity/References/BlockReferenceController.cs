using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class BlockReferenceController : MonoBehaviour
{
    Vector3 _referenceDirection;
    bool _transitionIsSet = false;
    List<AbstractEntityController> _entityReferences;
    List<AbstractEntityController> _entityCache;
    Vector3 _position;

    public List<AbstractEntityController> CalculateReferences3D(List<AbstractEntityController> entityCache, Vector3 position)
    {
        _entityCache = entityCache;
        _entityReferences = new List<AbstractEntityController> { null, null, null, null };
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

            _entityReferences[i] = null;
        }

        return _entityReferences;
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

    public void EvaluateUpperRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!(activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.up)))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    // Check ob Treppe bedeckt ist, oder über dem Spieler ein Block ist
                    bool foundCeil = false;
                    foreach (AbstractEntityController ceilingEntity in _entityCache)
                    {
                        if (ceilingEntity.GetPosition().Equals(_position + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }

                        if (ceilingEntity.GetPosition().Equals(_position + _referenceDirection + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }
                    }

                    if (foundCeil)
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }

    public void EvaluateMiddleRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!activeEntity.GetPosition().Equals(_position + _referenceDirection))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:
                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {activeEntity.GetEntityType()} sein");
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }
}