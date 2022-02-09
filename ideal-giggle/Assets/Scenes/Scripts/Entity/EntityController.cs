using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static EntityHelper;

public abstract class EntityController : MonoBehaviour
{
    protected EntityType _entityType;

    protected List<EntityController> _entityCache;

    protected EntityController[] _entityReferences = new EntityController[4];

    protected EntityReferenceCalculator _entityReferenceCalculator;

    protected Vector3 _position;


    public virtual void Awake()
    {
        for (int i = 0; i < _entityReferences.Length; i++)
        {
            _entityReferences[i] = null;
        }

        _entityReferenceCalculator = GetComponent<EntityReferenceCalculator>();

        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
        _position = transform.position;
    }

    public virtual void Start()
    {
        _entityCache = _entityReferenceCalculator.CacheSurroundingEntityReferences(_position);
        CalculateReferences();
    }

    public abstract void CalculateReferences();


    public Vector3 GetPosition()
    {
        return _position;
    }

    public EntityType GetEntityType()
    {
        return _entityType;
    }
    
    public EntityController[] GetEntityReferences()
    {
        return _entityReferences;
    }
}

