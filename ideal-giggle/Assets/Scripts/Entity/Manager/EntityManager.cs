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

    private Vector3 _pivot;


    public void Awake()
    {
        _entityList = new List<AbstractEntityController>(GetComponentsInChildren<AbstractEntityController>());
        _entityCalculator = GetComponent<EntityCalculator>();
        _pivot = new Vector3((_levelSize.x - 1) / 2f, (_levelSize.y - 1) / 2f, (_levelSize.z - 1) / 2f);
    }

    public void Start()
    {
        UpdateColor();
    }


    public void UpdateReferences()
    {
        List<AbstractEntityController> entityList;

        entityList = _entityCalculator.CalculateEntityList(_xPlane, _zPlane);

        //entityList.Where(e => e.GetEntityType().Equals(EntityType.STAIR)).Cast<StairController>().ToList().ForEach(s => Debug.Log($"STAIR: {s}, 2DEntityType: {s.GetEntityType2D()}"));

        foreach (AbstractEntityController entity in entityList)
        {
            entity.SetReferences(entityList);
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

    public Vector3 GetPivot()
    {
        return _pivot;
    }
}


