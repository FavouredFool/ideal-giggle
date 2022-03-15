using System.Collections.Generic;
using UnityEngine;

public class EntityCacheController : MonoBehaviour
{

    private EntityManager _entityManager;
    private Vector3 _position;
    private List<AbstractEntityController> _surroundingEntities;

    public void Awake()
    {
        _entityManager = GetComponentInParent<EntityManager>();
    }

    public List<AbstractEntityController> CacheEntityReferences3D(Vector3 position)
    {
        _position = position;
        _surroundingEntities = CacheSurroundingEntities();
        return _surroundingEntities;
    }

    public List<AbstractEntityController> CacheEntityReferences2D()
    {
        return null;
    }

    private List<AbstractEntityController> CacheSurroundingEntities()
    {
        List<AbstractEntityController> surroundingEntities = new List<AbstractEntityController>();

        foreach (AbstractEntityController entityController in _entityManager.GetEntityList())
        {

            if (entityController.GetPosition().Equals(transform.position))
            {
                continue;
            }

            int yInt = (int)entityController.GetPosition().y - (int)_position.y;
            bool yCheck = -1 <= yInt && yInt <= 2;

            if (!yCheck)
            {
                continue;
            }

            int xInt = (int)entityController.GetPosition().x - (int)_position.x;
            bool xCheck = -1 <= xInt && xInt <= 1;

            if (!xCheck)
            {
                continue;
            }

            int zInt = (int)entityController.GetPosition().z - (int)_position.z;
            bool zCheck = -1 <= zInt && zInt <= 1;

            if (!zCheck)
            {
                continue;
            }


            bool xEdgeCheck = xInt != 0;
            bool zEdgeCheck = zInt != 0;

            if (zEdgeCheck && xEdgeCheck)
            {
                continue;
            }

            surroundingEntities.Add(entityController);
        }

        return surroundingEntities;
    }

}
