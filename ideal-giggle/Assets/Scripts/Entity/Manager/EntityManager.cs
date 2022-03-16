using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

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


    public void Awake()
    {
        _entityList = new List<AbstractEntityController>(GetComponentsInChildren<AbstractEntityController>());
    }

    public void Start()
    {
        UpdateColor();
    }

    public void UpdateReferences(Dimension _dimension)
    {
        if (_dimension.Equals(Dimension.THREE))
        {
            foreach (AbstractEntityController entity in GetEntityList())
            {
                entity.SetReferences(_dimension, GetEntityList(), _xPlane, _zPlane);
            }
            return;
        }

        // Prune EntityList (This should go into a seperate class)
        List<AbstractEntityController> pruned2DList = new List<AbstractEntityController>();

        int xLevelSize = GetLevelSize().x;
        int yLevelSize = GetLevelSize().y;
        int zLevelSize = GetLevelSize().z;

        int width = 0;

        switch (_dimension)
        {
            case Dimension.TWO_X:
                width = zLevelSize;
                break;
            case Dimension.TWO_NX:
                width = zLevelSize;
                break;
            case Dimension.TWO_Z:
                width = xLevelSize;
                break;
            case Dimension.TWO_NZ:
                width = xLevelSize;
                break;
        }

        for (int i = 0; i < yLevelSize; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float temp = float.PositiveInfinity;
                AbstractEntityController tempEntity = null;

                // Durch jede Entity durchgehen um nur die Relevanten zu finden.
                foreach (AbstractEntityController entity in GetEntityList())
                {
                        
                    float widthPoint = 0;
                    float depthPoint = 0;
                    int negation = 0;

                    switch (_dimension)
                    {
                        case Dimension.TWO_X:
                            widthPoint = entity.transform.position.z;
                            depthPoint = entity.transform.position.x;
                            negation = -1;
                            break;
                        case Dimension.TWO_NX:
                            widthPoint = entity.transform.position.z;
                            depthPoint = entity.transform.position.x;
                            negation = 1;
                            break;
                        case Dimension.TWO_Z:
                            widthPoint = entity.transform.position.x;
                            depthPoint = entity.transform.position.z;
                            negation = -1;
                            break;
                        case Dimension.TWO_NZ:
                            widthPoint = entity.transform.position.x;
                            depthPoint = entity.transform.position.z;
                            negation = 1;
                            break;
                    }

                    bool horizontalGuard = widthPoint != j;
                    bool verticalGuard = entity.transform.position.y != i;

                    if (verticalGuard || horizontalGuard)
                    {
                        continue;
                    }

                    if (depthPoint*negation < temp)
                    {
                        temp = depthPoint*negation;
                        tempEntity = entity;
                    }
                }
                if (tempEntity)
                {
                    pruned2DList.Add(tempEntity);
                }
                

            }
        }
        
        foreach (AbstractEntityController entity in GetEntityList())
        {
            entity.SetReferences(_dimension, pruned2DList, _xPlane, _zPlane);
        }
        return;

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
