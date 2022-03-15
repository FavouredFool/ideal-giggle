using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static EntityHelper;
using static ViewHelper;

public abstract class AbstractEntityController : MonoBehaviour
{
    protected EntityType _entityType;

    protected List<AbstractEntityController> _entityCache;

    protected List<AbstractEntityController> _entityReferences2D = new List<AbstractEntityController> { null, null, null, null };

    protected List<AbstractEntityController> _entityReferences3D = new List<AbstractEntityController> { null, null, null, null };

    protected List<AbstractEntityController> _activeEntityReferences;

    protected SurroundingEntityCache3D _surroundingEntityCache3D;

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
        _surroundingEntityCache3D = GetComponent<SurroundingEntityCache3D>();
        _colorCalculator = GetComponent<ColorCalculator>();

        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
        _position = transform.position;
    }

    public void Start()
    {
        _entityCache = _surroundingEntityCache3D.CacheSurroundingEntityReferences(_position);
        _entityReferences3D = CalculateReferences3D();
        _activeEntityReferences = _entityReferences3D;
    }

    public abstract List<AbstractEntityController> CalculateReferences3D();

    public abstract List<AbstractEntityController> CalculateReferences2D(PlaneController xPlane, PlaneController zPlane);

    public void SetReferences(Dimension _dimension, PlaneController xPlane, PlaneController zPlane)
    {
        if (_dimension.Equals(Dimension.THREE))
        {
            _activeEntityReferences = _entityReferences3D;
        } else
        {
            _activeEntityReferences = CalculateReferences2D(xPlane, zPlane);
        }
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public EntityType GetEntityType()
    {
        return _entityType;
    }
    public List<AbstractEntityController> GetActiveEntityReferences()
    {
        return _activeEntityReferences;
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

    public ColorCalculator GetColorCalculator()
    {
        return _colorCalculator;
    }

    

}

