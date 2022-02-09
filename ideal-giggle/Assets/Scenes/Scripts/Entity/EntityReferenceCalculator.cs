using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class EntityReferenceCalculator : MonoBehaviour
{

    private EntityManager _entityManager;

    private Vector3 _position;

    private EntityController[] _entityReferences;

    private List<EntityController> _surroundingEntities;

    public void Awake()
    {
        _entityManager = GetComponentInParent<EntityManager>();
    }

    public List<EntityController> CacheSurroundingEntityReferences(Vector3 position)
    {
        _position = position;
        _surroundingEntities = CacheSurroundingEntities();
        return _surroundingEntities;
    }

    private List<EntityController> CacheSurroundingEntities()
    {
        List<EntityController> surroundingEntities = new List<EntityController>();

        foreach (EntityController entityController in _entityManager.GetEntityList())
        {

            if (entityController.Equals(this))
            {
                continue;
            }

            int yInt = (int)entityController.GetPosition().y - (int)_position.y;
            bool yCheck = -1 <= yInt && yInt <= 1;

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
            bool yEdgeCheck = yInt != 0;

            if (yEdgeCheck && xEdgeCheck)
            {
                continue;
            }

            surroundingEntities.Add(entityController);
        }

        return surroundingEntities;
    }

}
