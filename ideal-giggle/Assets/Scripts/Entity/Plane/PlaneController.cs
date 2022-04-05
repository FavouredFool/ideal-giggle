using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaneHelper;
using static TWODHelper;

public class PlaneController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerMovementController _playerMovement;

    [SerializeField]
    private PlaneType _planeType;


    public void MovePlane(Vector3 direction)
    {
        Vector3 desiredPosition = transform.position + direction;

        if (!MovementValid(desiredPosition))
        {
            return;
        }
        

        transform.localPosition = desiredPosition;
        _entityManager.UpdateColor();
        _entityManager.UpdateReferences();

    }

    protected bool MovementValid(Vector3 desiredPosition)
    {
        bool lowGuard = desiredPosition[GetPlaneCoordinateIndex(this)] < -0.5f;
        bool highGuard = desiredPosition[GetPlaneCoordinateIndex(this)] > _entityManager.GetLevelSize()[GetPlaneCoordinateIndex(this)] - 0.5f;

        if (lowGuard || highGuard)
        {
            return false;
        }

        bool playerToPlaneRelationBefore = transform.position[GetPlaneCoordinateIndex(this)] < _playerMovement.transform.position[GetPlaneCoordinateIndex(this)];
        bool playerToPlaneRelationAfter = desiredPosition[GetPlaneCoordinateIndex(this)] < _playerMovement.transform.position[GetPlaneCoordinateIndex(this)];

        if (!playerToPlaneRelationAfter.Equals(playerToPlaneRelationBefore))
        {
            return false;
        }


        return true;
    }

    public PlaneType GetPlaneType()
    {
        return _planeType;
    }

}
