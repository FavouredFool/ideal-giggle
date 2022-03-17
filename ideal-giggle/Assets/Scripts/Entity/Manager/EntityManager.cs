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

    private Dimension _dimension;


    public void Awake()
    {
        _entityList = new List<AbstractEntityController>(GetComponentsInChildren<AbstractEntityController>());
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
        }

        // Prune EntityList (This should go into a seperate class)
        List<AbstractEntityController> pruned2DList = new List<AbstractEntityController>();

        int xLevelSize = GetLevelSize().x;
        int yLevelSize = GetLevelSize().y;
        int zLevelSize = GetLevelSize().z;

        int width = 0;
        float activePlanePos = 0;
        int posDepthIndex = 0;
        int posWidthIndex = 0;
        int negation = 0;
        Vector3 lookDirection = Vector3.zero; ;

        switch (_dimension)
        {
            case Dimension.TWO_X:
                width = zLevelSize;
                activePlanePos = _xPlane.transform.position.x;
                posDepthIndex = 0;
                posWidthIndex = 2;
                negation = -1;
                lookDirection = Vector3.left;
                break;
            case Dimension.TWO_NX:
                width = zLevelSize;
                activePlanePos = _xPlane.transform.position.x;
                posDepthIndex = 0;
                posWidthIndex = 2;
                negation = 1;
                lookDirection = Vector3.right;
                break;
            case Dimension.TWO_Z:
                width = xLevelSize;
                activePlanePos = _zPlane.transform.position.z;
                posDepthIndex = 2;
                posWidthIndex = 0;
                negation = -1;
                lookDirection = Vector3.back;
                break;
            case Dimension.TWO_NZ:
                width = xLevelSize;
                activePlanePos = _zPlane.transform.position.z;
                posDepthIndex = 2;
                posWidthIndex = 0;
                negation = 1;
                lookDirection = Vector3.forward;
                break;
        }

        for (int i = 0; i < yLevelSize; i++)
        {
            for (int j = 0; j < width; j++)
            {
                List<AbstractEntityController> tempEntityList = new List<AbstractEntityController>();

                // Durch jede Entity durchgehen um nur die Relevanten zu finden.
                foreach (AbstractEntityController entity in GetEntityList())
                {
                    float widthPoint = entity.transform.position[posWidthIndex];
                    float depthPoint = entity.transform.position[posDepthIndex];

                    bool planeGuard = activePlanePos*negation < depthPoint*negation;

                    if (planeGuard)
                    {
                        continue;
                    }

                    bool horizontalGuard = widthPoint != j;
                    bool verticalGuard = entity.transform.position.y != i;

                    if (verticalGuard || horizontalGuard)
                    {
                        continue;
                    }

                    tempEntityList.Add(entity);
                }

                AbstractEntityController entityToAdd = null;

                if (tempEntityList.Count == 0)
                {
                    continue;
                }

                tempEntityList.Sort((AbstractEntityController x, AbstractEntityController y) =>
                {
                    if (x.transform.position[posDepthIndex]*negation > y.transform.position[posDepthIndex]*negation)
                    {
                        return 1;
                    } else if (x.transform.position[posDepthIndex]*negation < y.transform.position[posDepthIndex]*negation)
                    {
                        return -1;
                    } else
                    {
                        return 0;
                    }
                });

                // Check ob Entity eine schlecht rotierte Stair ist. Dann wird das hintere Element gewählt, welches dies nicht ist.
                entityToAdd = tempEntityList[0];

                for (int k = 0; k < tempEntityList.Count; k++)
                {
                    bool isStair = tempEntityList[k].GetEntityType().Equals(EntityType.STAIR);
                    
                    if (!isStair)
                    {
                        entityToAdd = tempEntityList[k];
                        break;
                    }

                    StairController stair = (StairController)tempEntityList[k];
                    bool isCorrectlyTurned = stair.GetTopEnter().V3Equal(lookDirection) || stair.GetTopEnter().V3Equal(-lookDirection);

                    if (isCorrectlyTurned)
                    {
                        entityToAdd = tempEntityList[k];
                        break;
                    }
                }

                pruned2DList.Add(entityToAdd);
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
