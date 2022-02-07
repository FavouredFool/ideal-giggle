using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
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
}
