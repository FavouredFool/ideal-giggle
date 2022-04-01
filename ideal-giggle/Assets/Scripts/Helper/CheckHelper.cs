using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static TWODHelper;

public class CheckHelper : MonoBehaviour
{
    protected static bool EntityCheck2D(AbstractEntityController entity, Vector3 position)
    {
        bool widthGuard = entity.GetPosition()[GetViewWidthIndex()].Equals(position[GetViewWidthIndex()]);
        bool heightGuard = entity.GetPosition().y.Equals(position.y);

        return widthGuard && heightGuard;
    }


    public static bool StairRotatedInDirection(Vector3 stairEnter, Vector3 referenceDirection)
    {
        return referenceDirection.Equals(stairEnter);
    }

    public static bool EntityExistsInList(List<AbstractEntityController> list, Vector3 checkPosition)
    {
        return list.Any(e => EntityExists(e, checkPosition));
    }

    public static AbstractEntityController EntityGetInList(List<AbstractEntityController> list, Vector3 checkPosition)
    {
        return list.Where(entity => EntityExists(entity, checkPosition)).FirstOrDefault();
    }

    public static bool EntityExists(AbstractEntityController entity, Vector3 checkPosition)
    {
        return entity.GetPosition().Equals(checkPosition);
    }
}
