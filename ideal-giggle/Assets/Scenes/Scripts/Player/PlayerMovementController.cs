using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerVisualController _visualController;

    [SerializeField]
    private EntityManager _entityManager;

    private PlayerStepCalculator _stepCalculator;

    private PlayerPathCalculator _pathCalculator;

    private Vector3 _endPosition = Vector3.negativeInfinity;

    private List<AbstractEntityController> _playerMovementPath;

    private bool _isMoving = false;
    private bool _pathCalculating = false;

    private AbstractEntityController _entityPlayerIsOn;
    private AbstractEntityController _goalEntity;

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

        if (_playerMovementPath == null)
        {
            return;
        }

        MoveAlongPath(_playerMovementPath);
    }

    public void MoveAlongPath(List<AbstractEntityController> movementPath)
    {
        Step step;

        _isMoving = true;

        step = _stepCalculator.CalculateStep(transform.position, movementPath);

        _visualController.MoveStep(step);
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        if (_pathCalculating)
        {
            return;
        }

        _endPosition = endPosition;
        Vector3 startPosition = transform.position;

        _entityPlayerIsOn = _entityManager.GetEntityFromCoordiantes(transform.position + Vector3.down);
        _goalEntity = _entityManager.GetEntityFromCoordiantes(_endPosition + Vector3.down);

        _playerMovementPath = null;
        StartCoroutine(CalculatePath());
    }

    public IEnumerator CalculatePath()
    {
        _pathCalculating = true;
        PathfinderCoroutine pc = new PathfinderCoroutine(this, _pathCalculator.CalculatePathAstar(_entityPlayerIsOn, _goalEntity));
        yield return pc.coroutine;
        _playerMovementPath = (List<AbstractEntityController>) pc.result;
        _pathCalculating = false;
        
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

public class PathfinderCoroutine
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;

    public PathfinderCoroutine(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while(target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }

}

