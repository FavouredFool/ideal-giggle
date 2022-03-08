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

    protected ColorCalculator _colorCalculator;

    protected Vector3 _position;

    protected Vector3 _visualPosition;

    // PathfinderStuff:
    public AbstractEntityController Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;



    public virtual void Awake()
    {
        _entityReferenceCalculator = GetComponent<SurroundingEntityCache>();
        _colorCalculator = GetComponent<ColorCalculator>();

        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
        _position = transform.position;
    }

    public void Start()
    {
        _colorCalculator.CalculateColor();
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

    public void ChangeMaterial(Material material)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = material;
        }
    }

    public Vector3 GetVisualPosition()
    {
        return _visualPosition;
    }

    public Vector3 GetAdjacentPosition(Vector3 addedVector)
    {
        return GetPosition() + addedVector;
    }

}

