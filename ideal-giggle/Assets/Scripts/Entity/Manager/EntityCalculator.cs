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

    public List<AbstractEntityController> CalculateEntityList(PlaneController xPlane, PlaneController zPlane)
    {
        List<AbstractEntityController> entityList;
        if (ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            entityList = _entityManager.GetEntityList();
            Set3DEntityTypes(entityList);
        }
        else
        {
            entityList = Calculate2DEntityList(xPlane, zPlane);
            Calculate2DEntityType(entityList);
        }

        return entityList;
    }

    public List<AbstractEntityController> Calculate2DEntityList(PlaneController xPlane, PlaneController zPlane)
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

    public void Calculate2DEntityType(List<AbstractEntityController> entityList)
    {
        foreach (AbstractEntityController entity in _entityManager.GetEntityList())
        {
            if (!entity.GetEntityType().Equals(EntityType.STAIR))
            {
                entity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (!entityList.Contains(entity))
            {
                entity.SetEntityType2D(EntityType.STAIR);
                continue;
            }

            StairController stairEntity = (StairController)entity;

            List<AbstractEntityController> depthList = GetEntityListFromPos2D(_entityManager.GetEntityList(), stairEntity.GetPosition());

            if (depthList.Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
            {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetTopEnter(), GetViewDirection())))
            {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetBottomEnter(), GetViewDirection())))
            {
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetBottomEnter(), stairEntity.GetTopEnter())))
            {
                stairEntity.SetEntityType2D(EntityType.INACCESSABLE);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetTopEnter(), stairEntity.GetBottomEnter())))
            {
                stairEntity.SetEntityType2D(EntityType.INACCESSABLE);
                continue;
            }

            stairEntity.SetEntityType2D(EntityType.STAIR);
        }
    }

    public void Set3DEntityTypes(List<AbstractEntityController> entityList)
    {
        entityList.ForEach(e => e.SetEntityType2D(e.GetEntityType()));
    }
}
