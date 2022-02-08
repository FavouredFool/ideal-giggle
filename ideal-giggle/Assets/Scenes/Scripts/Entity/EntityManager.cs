using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<EntityController> _entityList;

    public void Start()
    {
        _entityList = new List<EntityController>(GetComponentsInChildren<EntityController>());
    }

    public EntityController GetEntityFromCoordiantes(Vector3 coordinates)
    {
        EntityController entity = null;

        foreach (EntityController activeEntity in _entityList)
        {
            if (!activeEntity.GetPosition().Equals(coordinates)) {
                continue;
            }
            
            entity = activeEntity;
            break;
        }
        
        return entity;
    }


    public List<EntityController> GetEntityList()
    {
        return _entityList;
    }
}
