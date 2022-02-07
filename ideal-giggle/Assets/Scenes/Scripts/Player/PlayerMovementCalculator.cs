using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementCalculator : MonoBehaviour
{



    public Vector3 CalculateMovement(Vector3 activePosition, Vector3 endPosition)
    {
        int xActive = (int)activePosition.x;
        int xEnd = (int)endPosition.x;
        int zActive = (int)activePosition.z;
        int zEnd = (int)endPosition.z;

        int distX = Mathf.Abs(xActive - xEnd);
        int distZ = Mathf.Abs(zActive - zEnd);

        Vector3 movementVector = Vector3.zero;

        if (distX > distZ)
        {
            movementVector.x = Mathf.Sign(xEnd - xActive);
        }
        else if (distZ > distX)
        {
            movementVector.z = Mathf.Sign(zEnd - zActive);
        }
        else
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                movementVector.x = Mathf.Sign(xEnd - xActive);
            }
            else
            {
                movementVector.z = Mathf.Sign(zEnd - zActive);
            }
        }

        return movementVector;
    }
}