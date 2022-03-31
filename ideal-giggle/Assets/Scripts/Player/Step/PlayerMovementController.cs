using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static ViewHelper;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlayerVisualController _visualController;

    [SerializeField]
    private EntityManager _entityManager;

    private PlayerStepCalculator _stepCalculator;
    private PlayerPathCalculator _pathCalculator;

    private List<AbstractEntityController> _playerMovementPath;

    private bool _isMoving = false;
    private bool _pathCalculating = false;

    private AbstractEntityController _endEntity;
    private AbstractEntityController _groundEntity;

    public void Awake()
    {
        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
    }

    public void Start()
    {
        _stepCalculator = GetComponent<PlayerStepCalculator>();
        _pathCalculator = GetComponent<PlayerPathCalculator>();

        _groundEntity = _entityManager.GetEntityFromCoordiantes(transform.position + Vector3.down);
    }

    public void Update()
    {
        if (_groundEntity.Equals(_endEntity))
        {
            return;
        }

        if (_isMoving)
        {
            return;
        }

        if (_pathCalculating || _playerMovementPath == null)
        {
            return;
        }

        MoveAlongPath(_playerMovementPath);
    }

    public void MoveAlongPath(List<AbstractEntityController> movementPath)
    {
        Step step;

        _isMoving = true;

        step = _stepCalculator.CalculateStep(_groundEntity, movementPath);

        _visualController.MoveStep(step);
    }

    public void SetEndEntity(AbstractEntityController endEntity)
    {
        
        if (_pathCalculating)
        {
            return;
        }

        _endEntity = endEntity;
        _playerMovementPath = null;

        StartCoroutine(InitCalculatePath());
    }

    public IEnumerator InitCalculatePath()
    {
        _pathCalculating = true;

        PathfinderCoroutine pathfinderCoroutine = new PathfinderCoroutine(this, _pathCalculator.CalculatePathAstar(_groundEntity, _endEntity));
        yield return pathfinderCoroutine.coroutine;
        _playerMovementPath = (List<AbstractEntityController>) pathfinderCoroutine.result;

        _pathCalculating = false;
    }

    public void MovePlayerToFront()
    {
        if (_entityManager.GuardPlayerToFront(_groundEntity.GetPosition()))
        {
            return;
        }

        AbstractEntityController entity = _entityManager.GetFrontEntity(_groundEntity.GetPosition());

        MovePlayerToEntity(entity);
    }

    public void MovePlayerToEntity(AbstractEntityController entity)
    {
        Debug.Log($"FrontEntity: {entity}");

        transform.localPosition = entity.GetAdjacentPosition(Vector3.up);
        SetGroundEntity(entity);
        _endEntity = entity;

    }

    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    public AbstractEntityController GetGroundEntity()
    {
        return _groundEntity;
    }

    public void SetGroundEntity(AbstractEntityController groundEntity)
    {
        _groundEntity = groundEntity;
    }
}

