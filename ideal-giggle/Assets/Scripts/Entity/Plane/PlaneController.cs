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

    [Header("Plane Configurations")]
    [SerializeField]
    private PlaneType _planeType;

    [SerializeField]
    private float _startPos;

    [SerializeField]
    private float _planeWidth;

    

    public void Start()
    {
        ResizeAndPlacePlane();
    }

    private void ResizeAndPlacePlane()
    {
        Vector3Int levelSize = _entityManager.GetLevelSize();
        float planeSizeHeight = levelSize.y + 1;
        float planeSizeWidth = levelSize[GetPlaneCoordinateOtherIndex(this)] + 1f;

        Vector3 planeScale = Vector3.zero;
        planeScale[GetPlaneCoordinateOtherIndex(this)] = planeSizeWidth;
        planeScale.y = planeSizeHeight;
        planeScale[GetPlaneCoordinateIndex(this)] = _planeWidth;

        transform.localScale = planeScale;

        Vector3 planePosition = Vector3.zero;
        
        planePosition[GetPlaneCoordinateOtherIndex(this)] = (levelSize[GetPlaneCoordinateOtherIndex(this)] - 1f) / 2f;
        planePosition.y = (levelSize.y-1) / 2f;
        planePosition[GetPlaneCoordinateIndex(this)] = _startPos;

        transform.position = planePosition;
    }



    public void MovePlane(Vector3 desiredPosition)
    {
        transform.localPosition = desiredPosition;
        _entityManager.UpdateColor();
        _entityManager.UpdateReferences();

    }

    public bool MovementValid(Vector3 desiredPosition)
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

    public float GetStartPos()
    {
        return _startPos;
    }

}
