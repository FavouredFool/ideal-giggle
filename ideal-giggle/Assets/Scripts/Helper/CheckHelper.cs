using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TWODHelper;
using static ViewHelper;

public class CheckHelper : MonoBehaviour
{

    public static bool StairRotatedInDirection(Vector3 stairEnter, Vector3 referenceDirection)
    {
        return referenceDirection.Equals(stairEnter);
    }

    public static bool EntityExistsInList(List<AbstractEntityController> list, Vector3 checkPosition)
    {
        if (ActiveViewStateIsThreeD())
        {
            return list.Any(e => EntityExists3D(e, checkPosition));
        }
        else
        {
            return list.Any(e => EntityExists2D(e, checkPosition));
        }
    }

    public static AbstractEntityController GetEntityInListFromPos(List<AbstractEntityController> list, Vector3 checkPosition)
    {
        if (ActiveViewStateIsThreeD())
        {
            return list.Where(entity => EntityExists3D(entity, checkPosition)).FirstOrDefault();
        }
        else
        {
            return GetEntityListFromPos2D(list, checkPosition).FirstOrDefault();
        }
    }

    public static List<AbstractEntityController> GetEntityListFromPos2D(List<AbstractEntityController> list, Vector3 checkPosition)
    {
        
        List<AbstractEntityController> entityList;
        entityList = list.Where(entity => EntityExists2D(entity, checkPosition)).OrderBy(e => e.GetPosition()[GetViewDepthIndex()]).ToList();
       

        if (GetViewSign() > 0)
        {
            entityList.Reverse();
        }

        //Debug.Log("HERE: " + entityList.FirstOrDefault());

        return entityList;
    }

    protected static bool EntityExists3D(AbstractEntityController entity, Vector3 checkPosition)
    {
        return entity.GetPosition().Equals(checkPosition);
    }

    protected static bool EntityExists2D(AbstractEntityController entity, Vector3 checkPosition)
    {
        bool widthGuard = entity.GetPosition()[GetViewWidthIndex()].Equals(checkPosition[GetViewWidthIndex()]);
        bool heightGuard = entity.GetPosition().y.Equals(checkPosition.y);

        return widthGuard && heightGuard;
    }
}
