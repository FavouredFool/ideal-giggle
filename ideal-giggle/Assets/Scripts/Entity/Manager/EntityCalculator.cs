using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CheckHelper;
using static ViewHelper;
using static EntityHelper;
using static TWODHelper;

public class EntityCalculator : MonoBehaviour
{

    EntityManager _entityManager;

    private void Start()
    {
        _entityManager = GetComponent<EntityManager>();
    }

    public List<AbstractEntityController> Prune2DEntityList(PlaneController xPlane, PlaneController zPlane)
    {
        List<AbstractEntityController> pruned2DList = new List<AbstractEntityController>();

        int width = _entityManager.GetLevelSize()[GetViewWidthIndex()];
        int height = _entityManager.GetLevelSize()[1];
        int depth = _entityManager.GetLevelSize()[GetViewDepthIndex()];
        float activePlanePos = GetViewPlaneValue(xPlane, zPlane);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                AbstractEntityController entity = GetEntityInListFromPos(_entityManager.GetEntityList(), new Vector3(j, i, j));
                
                if (!entity)
                {
                    continue;
                }

                pruned2DList.Add(entity);
            }
        }

        return pruned2DList;
    }
}
