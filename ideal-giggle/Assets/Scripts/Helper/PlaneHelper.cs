using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TWODHelper;
using static PlaneHelper;
using static ViewHelper;

public class PlaneHelper : MonoBehaviour
{

    public enum PlaneType {  XPLANE, ZPLANE }
    
    public static bool EntityInFrontOfPlane(AbstractEntityController entity, PlaneController xPlane, PlaneController zPlane)
    {
        PlaneController viewPlane = GetViewPlane(xPlane, zPlane);

        if (viewPlane.transform.position[GetPlaneCoordinateIndex(viewPlane)] * GetViewSign() > entity.GetPosition()[GetViewDepthIndex()] * GetViewSign())
        {
            return false;
        }
        return true;
    }

    

    public static bool PlayerInFrontOfPlane(PlayerMovementController player, PlaneController plane)
    {

        if (plane.transform.position[GetPlaneCoordinateIndex(plane)] * GetViewSign() > player.transform.position[GetViewDepthIndex()] * GetViewSign())
        {
            return false;
        }
        return true;

    }

    public static int GetPlaneCoordinateIndex(PlaneController plane)
    {
        if (plane.GetPlaneType().Equals(PlaneType.XPLANE))
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    public static int GetPlaneCoordinateOtherIndex(PlaneController plane)
    {
        if (plane.GetPlaneType().Equals(PlaneType.XPLANE))
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public static PlaneController GetViewPlane(PlaneController xPlane, PlaneController zPlane)
    {
        PlaneController activePlane = null;

        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
            case Dimension.TWO_NX:
                activePlane = xPlane;
                break;
            case Dimension.TWO_Z:
            case Dimension.TWO_NZ:
                activePlane = zPlane;
                break;
            default:
                Debug.LogWarning("FEHLER");
                break;
        }

        return activePlane;
    }

}
