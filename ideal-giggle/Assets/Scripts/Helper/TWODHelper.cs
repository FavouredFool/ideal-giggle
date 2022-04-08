using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;
using static PlaneHelper;

public class TWODHelper : MonoBehaviour
{
    public static int GetViewWidthIndex()
    {
        int posWidthIndex = -1;
        switch (ActiveViewState)
        {
            case ViewState.X:
            case ViewState.NX:
                posWidthIndex = 2;
                break;

            case ViewState.Z:
            case ViewState.NZ:
                posWidthIndex = 0;
                break;
            default:
                Debug.Log("FEHLER");
                break;
        }

        return posWidthIndex;
    }

    public static int GetViewDepthIndex()
    {
        
        int posDepthIndex = -1;
        switch (ActiveViewState)
        {
            case ViewState.X:
            case ViewState.NX:
                posDepthIndex = 0;
                break;

            case ViewState.Z:
            case ViewState.NZ:
                posDepthIndex = 2;
                break;
            default:
                Debug.Log("FEHLER");
            break;
        }

        return posDepthIndex;
    }

    public static int GetViewSign()
    {
        int sign = 0;
        switch (ActiveViewState)
        {
            case ViewState.X:
            case ViewState.Z:
                sign = 1;
                break;

            case ViewState.NX:
            case ViewState.NZ:
                sign = -1;
                break;
            default:
                Debug.Log("FEHLER");
                break;
        }

        return sign;
    }

    




}
