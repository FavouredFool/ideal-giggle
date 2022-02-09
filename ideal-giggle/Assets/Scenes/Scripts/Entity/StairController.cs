using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StairController : EntityController
{

    bool _transitionIsSet = false;
    Vector3 _referenceDirection;

    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.STAIR;
    }

    public override void CalculateReferences()
    {

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
    }

    public Vector3 SwitchReferenceDirection(int index)
    {
        Vector3 referenceDirection;

        switch (index)
        {
            case 0:
                referenceDirection = Vector3.forward;
                break;
            case 1:
                referenceDirection = Vector3.right;
                break;
            case 2:
                referenceDirection = Vector3.back;
                break;
            case 3:
                referenceDirection = Vector3.left;
                break;
            default:
                referenceDirection = Vector3.zero;
                Debug.LogWarning($"FEHLER: referenceDirection darf niemals {referenceDirection} sein.");
                break;
        }

        return referenceDirection;
    }

    public void EvaluateUpperRow(int index)
    {
        foreach (EntityController activeEntity in _entityCache)
        {
            if (!(activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.up)))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.STAIR:
                    Debug.LogWarning("CHECK, OB BEIDE STAIRS KORREKT GEDREHT SIND, FEHLT");
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
        foreach (EntityController activeEntity in _entityCache)
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
                    Debug.LogWarning("CHECK, OB STAIRS KORREKT GEDREHT SIND, FEHLT");
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

    public void EvaluateLowerRow(int index)
    {
        foreach (EntityController activeEntity in _entityCache)
        {
            if (!activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.down))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:
                    Debug.LogWarning("CHECK, OB STAIR KORREKT GEDREHT IST, FEHLT");
                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:
                    Debug.LogWarning("CHECK, OB STAIRS KORREKT GEDREHT SIND, FEHLT");
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