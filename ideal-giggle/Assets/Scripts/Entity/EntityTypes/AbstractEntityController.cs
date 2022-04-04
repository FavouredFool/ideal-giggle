using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static EntityHelper;
using static ViewHelper;

public abstract class AbstractEntityController : MonoBehaviour
{
    protected EntityType _entityType;

    protected EntityType _entityType2D;

    protected List<AbstractEntityController> _entityCache;

    protected List<AbstractEntityController> _entityReferences2D = new List<AbstractEntityController> { null, null, null, null };

    protected List<EntityReference> _entityReferences3D = new List<EntityReference> { null, null, null, null };

    protected List<EntityReference> _activeEntityReferences;

    protected AbstractReferenceController3D _abstractReferenceController3D;

    protected ReferenceController2D _referenceController2D;

    protected EntityCacheController _entityCacheController;

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
        _entityCacheController = GetComponent<EntityCacheController>();
        _colorCalculator = GetComponent<ColorCalculator>();

        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
        _position = transform.position;
    }

    public void Start()
    {
        _abstractReferenceController3D = GetComponent<AbstractReferenceController3D>();
        _referenceController2D = GetComponent<ReferenceController2D>();
        _entityReferences3D = CalculateReferences3D();
        _activeEntityReferences = _entityReferences3D;
    }

    public List<EntityReference> CalculateReferences3D()
    {
        _entityCache = _entityCacheController.CacheEntityReferences3D(_position);
        return _abstractReferenceController3D.CalculateReferences3D(_entityCache, _position);
    }

    public List<EntityReference> CalculateReferences2D(List<AbstractEntityController> entityList)
    { 
        return _referenceController2D.CalculateReferences2D(entityList, _position);
    }

    public void SetReferences(List<AbstractEntityController> entityList, PlaneController xPlane, PlaneController zPlane)
    {
        if (ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            _activeEntityReferences = _entityReferences3D;
        } else
        {
            _activeEntityReferences = CalculateReferences2D(entityList);
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
    public List<EntityReference> GetActiveEntityReferences()
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
        if (ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            return _visualPosition;
        } else
        {
            switch (GetEntityType2D())
            {
                case EntityType.BLOCK:
                    return new Vector3(0, -0.25f, 0);
                    
                case EntityType.STAIR:
                    return new Vector3(0, -0.75f, 0);

                default:
                    Debug.LogWarning("Fehler");
                    return Vector3.zero;
            }
        }

        
    }

    public Vector3 GetAdjacentPosition(Vector3 addedVector)
    {
        return GetPosition() + addedVector;
    }

    public ColorCalculator GetColorCalculator()
    {
        return _colorCalculator;
    }

    public EntityType GetEntityType2D()
    {
        return _entityType2D;
    }

    public void SetEntityType2D(EntityType entityType2D)
    {
        _entityType2D = entityType2D;
    }



}

