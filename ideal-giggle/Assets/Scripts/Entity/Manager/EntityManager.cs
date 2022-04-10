using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ViewHelper;
using static EntityHelper;
using static TWODHelper;
using static CheckHelper;
using static PlaneHelper;

public class EntityManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Material _entityMaterial;

    [SerializeField]
    private PlaneController _xPlane;

    [SerializeField]
    private PlaneController _zPlane;

    [SerializeField]
    private PlayerMovementController _playerMovementController;

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


    public void UpdateReferencePipeline(ViewState desiredViewState)
    {
        // Wenn aus 2D heraus geswapped wird
        if (!ActiveViewStateIsThreeD())
        {
            _playerMovementController.MovePlayerToFront(true);
        }

        ActiveViewState = desiredViewState;
        UpdateColor();
        UpdateReferences();

        // Wenn in 2D herein geswapped wurde
        if (!ActiveViewStateIsThreeD())
        {
            _playerMovementController.MovePlayerToFront(false);
        }
    }

    public void UpdateReferences()
    {
        List<AbstractEntityController> entityList;

        entityList = _entityCalculator.CalculateEntityList(_xPlane, _zPlane);

        foreach (AbstractEntityController entity in entityList)
        {
            entity.SetReferences(entityList);
        }
    }

    public bool InputValid()
    {
        if (!ActiveViewStateIsThreeD())
        {
            if (_playerMovementController.GetGroundEntity().GetEntityType2D().Equals(EntityType.BLOCK))
            {
                if (_playerMovementController.GetEntityPositionRelation().Equals(EntityType.BLOCK))
                {
                    if (!(GetEntityListFromPos2D(GetEntityList(), _playerMovementController.GetGroundEntity().GetPosition()).Any(e => e.GetEntityType().Equals(EntityType.BLOCK))))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
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


