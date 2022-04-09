using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static ViewHelper;
using static EntityHelper;
using static PlaneHelper;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlaneController _xPlane;

    [SerializeField]
    private PlaneController _zPlane;


    private PlayerVisualController _visualController;
    private PlayerStepCalculator _stepCalculator;
    private PlayerPathCalculator _pathCalculator;
    private PlayerToFrontCalculator _playerToFrontCalculator;

    private List<AbstractEntityController> _playerMovementPath;

    private bool _isMoving = false;
    private bool _pathCalculating = false;
    private bool _isBlocked = false;

    private AbstractEntityController _endEntity;
    private AbstractEntityController _groundEntity;

    private EntityType _entityPositionRelation = EntityType.BLOCK;

    public void Awake()
    {
        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
    }

    public void Start()
    {
        _visualController = GetComponentInChildren<PlayerVisualController>();
        _stepCalculator = GetComponent<PlayerStepCalculator>();
        _pathCalculator = GetComponent<PlayerPathCalculator>();
        _playerToFrontCalculator = GetComponent<PlayerToFrontCalculator>();

        _groundEntity = _entityManager.GetEntityFromCoordiantes(transform.position + Vector3.down);
    }

    public void Update()
    {
        CalculateBlocked();

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

        if (_isBlocked)
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
        if (_isBlocked)
        {
            return;
        }

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

    public void MovePlayerToFront(bool relative)
    {
        _playerToFrontCalculator.MovePlayerToFront(relative);
    }

    public void MovePlayerToEntity(AbstractEntityController entity)
    {
        transform.localPosition = entity.GetAdjacentPosition(Vector3.up);
        SetGroundEntity(entity);
        _endEntity = entity;

    }

    public bool CalculateBlocked()
    {
        if (!ActiveViewStateIsThreeD())
        {
            if (!_groundEntity.GetEntityType2D().Equals(_entityPositionRelation))
            {
                _isBlocked = true;
                return _isBlocked;
            }

            if (!PlayerInFrontOfPlane(this, GetViewPlane(_xPlane, _zPlane)))
            {
                _isBlocked = true;
                return _isBlocked;
            }
        }

        _isBlocked = false;
        return _isBlocked;
    }

    public void InterpretHitInput(Vector3 hit)
    {
        AbstractEntityController hitEntity = _entityManager.GetEntityFromCoordiantes(hit);

        if (!hitEntity)
        {
            throw new Exception();
        }

        SetEndEntity(hitEntity);
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

    public EntityType GetEntityPositionRelation()
    {
        return _entityPositionRelation;
    }

    public void SetEntityPositionRelation(EntityType entityPositionRelation)
    {
        _entityPositionRelation = entityPositionRelation;
    }
}



