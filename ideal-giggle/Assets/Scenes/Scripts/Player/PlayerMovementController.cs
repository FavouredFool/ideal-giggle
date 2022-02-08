using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerVisualController _visualController;

    private PlayerStepCalculator _stepCalculator;

    private Vector3 _endPosition = Vector3.negativeInfinity;

    private bool _isMoving = false;

    public void Awake()
    {
        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
    }

    public void Start()
    {
        _stepCalculator = GetComponent<PlayerStepCalculator>();
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
        Step step;

        _isMoving = true;

        step = _stepCalculator.CalculateStep(transform.position, endPosition);

        _visualController.MoveStep(step);
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


