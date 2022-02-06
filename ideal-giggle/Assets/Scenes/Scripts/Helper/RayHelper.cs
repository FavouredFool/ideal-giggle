using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHelper : MonoBehaviour
{
    public static Vector3 CastRayFromMousePosition()
    {
        Debug.LogWarning("Achtung: Hier Referenziere ich nur die MainCamera. Das k�nnte zu Problemen f�hren.");
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.negativeInfinity;
        }
    }
}
