using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationController _animationController;

    private PlayerMovementCalculator _movementCalculator;

    private Vector3 _endPosition = Vector3.negativeInfinity;

    private bool _isMoving = false;


    public void Start()
    {
        _movementCalculator = GetComponent<PlayerMovementCalculator>();
    }

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
        Vector3 movement;

        _isMoving = true;

        movement = _movementCalculator.CalculateMovement(transform.position, endPosition);

        _animationController.MoveStep(movement);
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


