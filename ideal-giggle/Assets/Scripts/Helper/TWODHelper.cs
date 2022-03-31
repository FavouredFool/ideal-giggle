using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class TWODHelper : MonoBehaviour
{
    public static int GetViewWidthIndex()
    {
        int posWidthIndex = -1;
        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
            case Dimension.TWO_NX:
                posWidthIndex = 2;
                break;

            case Dimension.TWO_Z:
            case Dimension.TWO_NZ:
                posWidthIndex = 0;
                break;
            case Dimension.THREE:
                Debug.Log("FEHLER");
                break;
        }

        return posWidthIndex;
    }

    public static int GetViewDepthIndex()
    {
        int posDepthIndex = -1;
        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
            case Dimension.TWO_NX:
                posDepthIndex = 0;
                break;

            case Dimension.TWO_Z:
            case Dimension.TWO_NZ:
                posDepthIndex = 2;
                break;
            case Dimension.THREE:
                Debug.Log("FEHLER");
                break;
        }

        return posDepthIndex;
    }

    public static int GetViewSign()
    {
        int sign = 0;
        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
            case Dimension.TWO_Z:
            
                sign = 1;
                break;

            case Dimension.TWO_NX:
            case Dimension.TWO_NZ:
                sign = -1;
                break;
            case Dimension.THREE:
                Debug.Log("FEHLER");
                break;
        }

        return sign;
    }

    public static Vector3 GetViewDirection()
    {
        Vector3 direction = Vector3.zero;
        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
                direction = Vector3.left;
                break;
            case Dimension.TWO_NX:
                direction = Vector3.right;
                break;
            case Dimension.TWO_Z:
                direction = Vector3.back;
                break;
            case Dimension.TWO_NZ:
                direction = Vector3.forward;
                break;
            case Dimension.THREE:
                Debug.Log("FEHLER");
                break;
        }

        return direction;
    }

    public static float GetViewPlaneValue(PlaneController xPlane, PlaneController zPlane)
    {
        float activePlaneValue = -1;

        switch (ViewDimension.Dimension)
        {
            case Dimension.TWO_X:
                activePlaneValue = xPlane.transform.position.x;
                break;
            case Dimension.TWO_NX:
                activePlaneValue = xPlane.transform.position.x;
                break;
            case Dimension.TWO_Z:
                activePlaneValue = zPlane.transform.position.z;
                break;
            case Dimension.TWO_NZ:
                activePlaneValue = zPlane.transform.position.z;
                break;
        }

        return activePlaneValue;
    }
}
