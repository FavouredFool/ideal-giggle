using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
        }

        foreach (AbstractEntityController entity in GetEntityList())
        {
            entity.SetReferences(entityList, _xPlane, _zPlane);
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

    public AbstractEntityController GetFrontEntity(AbstractEntityController entity, EntityType searchedType)
    {
        int posDepthIndex = -1;
        int sign = 0;
        Vector3 position = entity.GetPosition();
        Vector3 direction = Vector3.zero;

        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
                posDepthIndex = 0;
                sign = 1;
                direction = Vector3.left;
                break;
            case Dimension.TWO_NX:
                posDepthIndex = 0;
                sign = -1;
                direction = Vector3.right;
                break;

            case Dimension.TWO_Z:
                posDepthIndex = 2;
                sign = 1;
                direction = Vector3.back;
                break;
            case Dimension.TWO_NZ:
                posDepthIndex = 2;
                sign = -1;
                direction = Vector3.forward;
                break;
        }

        if (searchedType.Equals(EntityType.BLOCK))
        {
            if (sign > 0)
            {
                return GetEntityList().Where(allEntities => allEntities.GetEntityType().Equals(searchedType)).Where(entity => EntityCheck2D(entity, position)).OrderByDescending(e => e.GetPosition()[posDepthIndex]).FirstOrDefault();
            }
            else
            {
                return GetEntityList().Where(allEntities => allEntities.GetEntityType().Equals(searchedType)).Where(entity => EntityCheck2D(entity, position)).OrderBy(e => e.GetPosition()[posDepthIndex]).FirstOrDefault();
            }
        } else if (searchedType.Equals(EntityType.STAIR))
        {
            List<AbstractEntityController> list;
            if (sign > 0)
            {
                list = GetEntityList().Where(entity => EntityCheck2D(entity, position)).OrderByDescending(e => e.GetPosition()[posDepthIndex]).ToList();
            }
            else
            {
                list = GetEntityList().Where(entity => EntityCheck2D(entity, position)).OrderBy(e => e.GetPosition()[posDepthIndex]).ToList();
            }

            if (list.Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
            {
                // Es gibt einen Block in der Reihe, weshalb man die originale Entity zurück gibt
                return entity;
            } else
            {
                // Check ob alle Stairs korrekt gedreht sind
                if (list.Any(e => WronglyTurnedStair(e, direction)))
                {
                    return entity;
                }

            }

            return list.FirstOrDefault();

        }

        return null;
        
    }

    protected bool WronglyTurnedStair(AbstractEntityController e, Vector3 direction)
    {
        StairController stair = (StairController)e;

        if (stair.GetBottomEnter().V3Equal(direction) || stair.GetTopEnter().V3Equal(direction))
        {
            return true;
        }

        return false;
    }


    protected bool EntityExists(AbstractEntityController entity, Vector3 position)
    {
        return entity.GetPosition().Equals(position);
    }

    public bool GuardPlayerToFront(Vector3 groundEntityPosition)
    {
        return GetEntityList().Any(e => EntityCheck2D(e, groundEntityPosition + Vector3.up));
    }

    protected bool EntityCheck2D(AbstractEntityController entity, Vector3 position)
    {
        int posWidthIndex = -1;

        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
            case Dimension.TWO_NX:
                posWidthIndex = 2;
                break;

            case Dimension.TWO_Z:
            case Dimension.TWO_NZ:
                posWidthIndex = 0;
                break;
        }

        bool widthGuard = entity.GetPosition()[posWidthIndex].Equals(position[posWidthIndex]);
        bool heightGuard = entity.GetPosition().y.Equals(position.y);

        return widthGuard && heightGuard;
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


