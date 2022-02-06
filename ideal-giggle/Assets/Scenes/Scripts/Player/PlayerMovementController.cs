using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationController _animationController;

    private Vector3 _endPosition = Vector3.negativeInfinity;

    private bool _isMoving = false;


    public void Update()
    {
        if (_endPosition.Equals(Vector3.negativeInfinity))
        {
            return;
        }

        if (transform.position.Equals(_endPosition))
        {
            return;
        }

        if (_isMoving)
        {
            return;
        }

        MoveTo(_endPosition);
    }

    public void MoveTo(Vector3 endPosition)
    {
        _isMoving = true;

        Vector3 activePosition = CoordinateHelper.DetermineGridCoordinate(transform.position);
        transform.position = activePosition;

        Vector3 movement = CalculateMovement(activePosition, endPosition);

        _animationController.MoveStep(movement);
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

    public void SetEndPosition(Vector3 endPosition)
    {
        _endPosition = endPosition;
    }

    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }
}
