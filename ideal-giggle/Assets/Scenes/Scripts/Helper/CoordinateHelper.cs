using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateHelper : MonoBehaviour
{

    public static Vector3 DetermineGridCoordinate(Vector3 coordinate)
    {

        coordinate = RoundVector(coordinate);

        return coordinate;

    }

    public static Vector3 RoundVector(Vector3 coordinate)
    {
        coordinate.x = Mathf.Round(coordinate.x);
        coordinate.y = Mathf.Ceil(coordinate.y);
        coordinate.z = Mathf.Round(coordinate.z);

        return coordinate;
    }
}
