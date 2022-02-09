using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static EntityHelper;

public abstract class EntityController : MonoBehaviour
{
    protected EntityType _entityType;

    protected List<EntityController> _entityCache;

    protected List<EntityController> _entityReferences = new List<EntityController> { null, null, null, null };

    protected SurroundingEntityCache _entityReferenceCalculator;

    protected Vector3 _position;


    public virtual void Awake()
    {
        _entityReferenceCalculator = GetComponent<SurroundingEntityCache>();

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
    public List<EntityController> GetEntityReferences()
    {
        return _entityReferences;
    }
}

