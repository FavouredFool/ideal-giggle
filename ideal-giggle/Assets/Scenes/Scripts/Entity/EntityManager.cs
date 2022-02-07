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


    public List<EntityController> GetEntityList()
    {
        return _entityList;
    }
}
