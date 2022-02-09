using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StairController : EntityController
{
    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.STAIR;
    }

    public override void CalculateReferences()
    {
        // EntityTransitions
        _entityReferences = new EntityController[4];

        //_entityCache

        for (int i = 0; i < 4; i++)
        {
            Vector3 referenceDirection;
            switch (i)
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

            // Check Transitions
            bool transitionIsSet = false;
            foreach (EntityController activeEntity in _entityCache)
            {
                if (!(activeEntity.GetPosition().Equals(_position + referenceDirection + Vector3.up)))
                {
                    continue;
                }

                if (activeEntity.GetEntityType().Equals(EntityType.STAIR))
                {
                    Debug.LogWarning("CHECK, OB STAIR KORREKT GEDREHT IST, FEHLT");
                    _entityReferences[i] = activeEntity;
                }
                else
                {
                    _entityReferences[i] = null;
                }

                transitionIsSet = true;
                break;
            }

            if (transitionIsSet)
            {
                continue;
            }

            foreach (EntityController activeEntity in _entityCache)
            {
                if (!activeEntity.GetPosition().Equals(_position + referenceDirection))
                {
                    continue;
                }

                transitionIsSet = true;

                switch (activeEntity.GetEntityType())
                {
                    case EntityType.BLOCK:
                        _entityReferences[i] = activeEntity;
                        break;
                    case EntityType.STAIR:
                        Debug.LogWarning("CHECK, OB STAIR KORREKT GEDREHT IST, FEHLT");
                        _entityReferences[i] = activeEntity;
                        break;
                    default:
                        Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {activeEntity.GetEntityType()} sein");
                        _entityReferences[i] = null;
                        break;
                }

                break;
            }

            if (!transitionIsSet)
            {
                _entityReferences[i] = null;
            }
        }
    }
}