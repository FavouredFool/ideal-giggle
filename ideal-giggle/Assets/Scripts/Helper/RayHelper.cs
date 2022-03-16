using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHelper : MonoBehaviour
{
    public static Vector3 CastRayFromMousePosition()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            // Additional 0.01f so it doesn't hit the exact side of a cube but rather a bit deeper inside.
            return hit.point + inputRay.direction.normalized * 0.01f;
        }
        else
        {
            return Vector3.negativeInfinity;
        }
    }
}
