using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CheckHelper;
using static ViewHelper;
using static EntityHelper;
using static TWODHelper;
using static PlaneHelper;

public class EntityCalculator : MonoBehaviour
{

    EntityManager _entityManager;

    private void Awake()
    {
        _entityManager = GetComponent<EntityManager>();
    }

    public List<AbstractEntityController> CalculateEntityList(PlaneController xPlane, PlaneController zPlane)
    {
        List<AbstractEntityController> entityList;
        if (ActiveViewStateIsThreeD())
        {
            entityList = _entityManager.GetEntityList();
            Set3DEntityTypes(entityList);
        }
        else
        {
            PlaneController plane = GetViewPlane(xPlane, zPlane);
            entityList = Calculate2DEntityList(plane);
            Calculate2DEntityType(entityList, plane);
        }

        return entityList;
    }

    public List<AbstractEntityController> Calculate2DEntityList(PlaneController plane)
    {
        List<AbstractEntityController> pruned2DList = new List<AbstractEntityController>();
        int width = _entityManager.GetLevelSize()[GetViewWidthIndex()];
        int height = _entityManager.GetLevelSize()[1];
        int depth = _entityManager.GetLevelSize()[GetViewDepthIndex()];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                AbstractEntityController entity = GetEntityInListFromPos(_entityManager.GetEntityList(), new Vector3(j, i, j));
                
                if (!entity)
                {
                    continue;
                }

                if (!EntityInFrontOfPlane(entity, plane))
                {
                    continue;
                }

                pruned2DList.Add(entity);
            }
        }

        return pruned2DList;
    }

    public void Calculate2DEntityType(List<AbstractEntityController> entityList, PlaneController plane)
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

            List<AbstractEntityController> depthList = GetEntityListFromPos2DIncludingPlaneGuard(_entityManager.GetEntityList(), stairEntity.GetPosition(), plane);

            if (depthList.Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
            {
                Debug.Log("AA");
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetTopEnter(), GetViewDirectionNormalized(ActiveViewState))))
            {
                Debug.Log("AA");
                stairEntity.SetEntityType2D(EntityType.BLOCK);
                continue;
            }

            if (depthList.Cast<StairController>().Any(s => StairRotatedInDirection(s.GetBottomEnter(), GetViewDirectionNormalized(ActiveViewState))))
            {
                Debug.Log("AA"); Debug.Log("AA");
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
