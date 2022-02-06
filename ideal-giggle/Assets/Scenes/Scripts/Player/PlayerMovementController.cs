using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{

    [SerializeField]
    private AbstractStep[] _steps;

    public void Start()
    {
        gameObject.GetComponent<PlayerInputController>();
    }

    public void MoveTo(Vector3 endPosition)
    {
        Vector3 activePosition = CoordinateHelper.DetermineGridCoordinate(transform.position);
        transform.position = activePosition;

        int loopCounter = 0;
        while (activePosition != endPosition)
        {
            if (loopCounter > 1000)
            {
                Debug.LogWarning("Endlosschleife entkommen");
                break;
            }

            loopCounter++;

            AbstractStep step;
            step = CalculateStep(activePosition, endPosition);

            step.MoveStep();

            activePosition = transform.position;
        }

    }

    public AbstractStep CalculateStep(Vector3 activePosition, Vector3 endPosition)
    {
        AbstractStep step = _steps[0];

        step.Movement = CalculateMovement(activePosition, endPosition);

        return step;        
    }

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
