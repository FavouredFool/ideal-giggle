using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class EntityController : MonoBehaviour
{
    [SerializeField]
    private EntityType _entityType;

    private Vector3 _position;

    public void Awake()
    {
        transform.position = CoordinateHelper.DetermineGridCoordinate(transform.position);
        _position = transform.position;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public EntityType GetEntityType()
    {
        return _entityType;
    }
}
