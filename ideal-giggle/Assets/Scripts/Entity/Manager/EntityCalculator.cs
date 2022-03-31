using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                List<AbstractEntityController> tempEntityList = new List<AbstractEntityController>();

                foreach (AbstractEntityController entity in _entityManager.GetEntityList())
                {
                    float widthPoint = entity.transform.position[GetViewWidthIndex()];
                    float depthPoint = entity.transform.position[GetViewDepthIndex()];

                    /* PLANE GUARD RAUSGENOMMEN
                    bool planeGuard = activePlanePos * negation < depthPoint * negation;

                    if (planeGuard)
                    {
                        continue;
                    }
                    */

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
                if (x.transform.position[GetViewDepthIndex()] * -GetViewSign() > y.transform.position[GetViewDepthIndex()] * -GetViewSign())
                    {
                        return 1;
                    }
                    else if (x.transform.position[GetViewDepthIndex()] * -GetViewSign() < y.transform.position[GetViewDepthIndex()] * -GetViewSign())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                });

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
                    bool isCorrectlyTurned = stair.GetTopEnter().V3Equal(GetViewDirection()) || stair.GetTopEnter().V3Equal(-GetViewDirection());

                    if (isCorrectlyTurned)
                    {
                        entityToAdd = tempEntityList[k];
                        break;
                    }
                }
                pruned2DList.Add(entityToAdd);
            }
        }

        return pruned2DList;
    }
}
