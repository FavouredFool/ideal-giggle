using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;
using static EntityHelper;

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

        int xLevelSize = _entityManager.GetLevelSize().x;
        int yLevelSize = _entityManager.GetLevelSize().y;
        int zLevelSize = _entityManager.GetLevelSize().z;

        int width = 0;
        float activePlanePos = 0;
        int posDepthIndex = 0;
        int posWidthIndex = 0;
        int negation = 0;
        Vector3 lookDirection = Vector3.zero; ;

        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
                width = zLevelSize;
                activePlanePos = xPlane.transform.position.x;
                posDepthIndex = 0;
                posWidthIndex = 2;
                negation = -1;
                lookDirection = Vector3.left;
                break;
            case Dimension.TWO_NX:
                width = zLevelSize;
                activePlanePos = xPlane.transform.position.x;
                posDepthIndex = 0;
                posWidthIndex = 2;
                negation = 1;
                lookDirection = Vector3.right;
                break;
            case Dimension.TWO_Z:
                width = xLevelSize;
                activePlanePos = zPlane.transform.position.z;
                posDepthIndex = 2;
                posWidthIndex = 0;
                negation = -1;
                lookDirection = Vector3.back;
                break;
            case Dimension.TWO_NZ:
                width = xLevelSize;
                activePlanePos = zPlane.transform.position.z;
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
                foreach (AbstractEntityController entity in _entityManager.GetEntityList())
                {
                    float widthPoint = entity.transform.position[posWidthIndex];
                    float depthPoint = entity.transform.position[posDepthIndex];

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
                    if (x.transform.position[posDepthIndex] * negation > y.transform.position[posDepthIndex] * negation)
                    {
                        return 1;
                    }
                    else if (x.transform.position[posDepthIndex] * negation < y.transform.position[posDepthIndex] * negation)
                    {
                        return -1;
                    }
                    else
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

        return pruned2DList;
    }
}
