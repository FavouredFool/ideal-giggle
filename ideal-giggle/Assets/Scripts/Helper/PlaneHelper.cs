using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TWODHelper;
using static PlaneHelper;
using static ViewHelper;

public class PlaneHelper : MonoBehaviour
{

    public enum PlaneType {  XPLANE, ZPLANE }
    
    public static bool EntityInFrontOfPlane(AbstractEntityController entity, PlaneController viewPlane)
    {
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

        switch (ActiveViewState)
        {
            case ViewState.X:
            case ViewState.NX:
                activePlane = xPlane;
                break;
            case ViewState.Z:
            case ViewState.NZ:
                activePlane = zPlane;
                break;
            default:
                Debug.LogWarning("FEHLER");
                break;
        }

        return activePlane;
    }

    public static int GetSliderRotation()
    {

        switch (ActiveViewState)
        {
            case ViewState.X:
                return 0;
            case ViewState.NZ:
                return 90;
            case ViewState.NX:
                return 180;
            case ViewState.Z:
                return 270;

            case ViewState.X_NZ:
                return 45;
            case ViewState.NZ_NX:
                return 135;
            case ViewState.NX_Z:
                return 225;
            case ViewState.Z_X:
                return 315;
        }

        return -1;
    }

}
