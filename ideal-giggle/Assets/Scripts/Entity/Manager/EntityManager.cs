using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;
using static EntityHelper;

public class EntityManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Material _entityMaterial;

    [SerializeField]
    private PlaneController _xPlane;

    [SerializeField]
    private PlaneController _zPlane;

    [Header("Dimensions")]
    [SerializeField]
    private Vector3Int _levelSize;

    private List<AbstractEntityController> _entityList;
    private EntityCalculator _entityCalculator;

    private Dimension _dimension;


    public void Awake()
    {
        _entityList = new List<AbstractEntityController>(GetComponentsInChildren<AbstractEntityController>());
        _entityCalculator = GetComponent<EntityCalculator>();
    }

    public void Start()
    {
        UpdateColor();
    }

    public void UpdateReferences()
    {
        UpdateReferences(_dimension);
    }

    public void UpdateReferences(Dimension dimension)
    {
        _dimension = dimension;

        if (_dimension.Equals(Dimension.THREE))
        {
            foreach (AbstractEntityController entity in GetEntityList())
            {
                entity.SetReferences(_dimension, GetEntityList(), _xPlane, _zPlane);
            }
            return;
        } else
        {
            List<AbstractEntityController> pruned2DList = _entityCalculator.Prune2DEntityList(_dimension, _xPlane, _zPlane);

            foreach (AbstractEntityController entity in pruned2DList)
            {
                entity.SetReferences(dimension, pruned2DList, _xPlane, _zPlane);
            }
        }
    }

    public AbstractEntityController GetEntityFromCoordiantes(Vector3 coordinates)
    {
        AbstractEntityController entity = null;

        foreach (AbstractEntityController activeEntity in _entityList)
        {
            if (!activeEntity.GetPosition().Equals(coordinates)) {
                continue;
            }
            
            entity = activeEntity;
            break;
        }
        
        return entity;
    }

    public void UpdateColor()
    {
        foreach (AbstractEntityController ele in _entityList)
        {
            ele.GetColorCalculator().CalculateColor(_xPlane.transform.position.x, _zPlane.transform.position.z);
        }
    }

    public List<AbstractEntityController> GetEntityList()
    {
        return _entityList;
    }

    public Material GetEntityMaterial()
    {
        return _entityMaterial;
    }

    public Vector3Int GetLevelSize()
    {
        return _levelSize;
    }
}


