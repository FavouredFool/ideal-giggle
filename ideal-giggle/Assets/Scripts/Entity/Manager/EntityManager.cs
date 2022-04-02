using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static EntityHelper;
using static TWODHelper;
using static CheckHelper;

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
        List<AbstractEntityController> entityList;

        if (ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            entityList = GetEntityList();
        } else
        {
            entityList = _entityCalculator.Prune2DEntityList(_xPlane, _zPlane);

            Set2DEntityTypes(entityList);
        }

        //entityList.Where(e => e.GetEntityType().Equals(EntityType.STAIR)).Cast<StairController>().ToList().ForEach(s => Debug.Log($"STAIR: {s}, 2DEntityType: {s.GetEntityType2D()}"));

        foreach (AbstractEntityController entity in GetEntityList())
        {
            entity.SetReferences(entityList, _xPlane, _zPlane);
        }
    }
    
    public void Set2DEntityTypes(List<AbstractEntityController> entityList)
    {
        foreach(AbstractEntityController entity in GetEntityList())
        {
            if (!entity.GetEntityType().Equals(EntityType.STAIR))
            {
                continue;
            }

            StairController stairEntity = (StairController)entity;

            if (!entityList.Contains(stairEntity))
            {
                stairEntity.SetEntityType2D(stairEntity.GetEntityType());
                continue;
            }

            List<AbstractEntityController> depthList = GetEntityListFromPos2D(GetEntityList(), stairEntity.GetPosition());

            if (depthList.Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
            {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetTopEnter(), GetViewDirection()))) {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetBottomEnter(), GetViewDirection())))
            {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            stairEntity.SetEntityType2D(EntityType.STAIR);
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


