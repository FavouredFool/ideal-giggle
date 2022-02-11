using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static EntityHelper;

public abstract class AbstractEntityController : MonoBehaviour
{
    protected EntityType _entityType;

    protected List<AbstractEntityController> _entityCache;

    protected List<AbstractEntityController> _entityReferences = new List<AbstractEntityController> { null, null, null, null };

    protected SurroundingEntityCache _entityReferenceCalculator;

    protected Vector3 _position;

    // PathfinderStuff:
    public AbstractEntityController Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;



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
    public List<AbstractEntityController> GetEntityReferences()
    {
        return _entityReferences;
    }

    public void PathfinderInit()
    {
        // Set H and G
    }

    public void SetConnection(AbstractEntityController entity)
    {
        Connection = entity;
    }

    public void SetG(float g)
    {
        G = g;
    }

    public void SetH(float h)
    {
        H = h;
    }

}

