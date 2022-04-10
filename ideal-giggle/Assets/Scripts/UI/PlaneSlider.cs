using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using static ViewHelper;
using static PlaneHelper;

public class PlaneSlider : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlaneController _affectedPlane;

    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private CameraMovement _cameraMovement;

    private Slider _slider;

    private float oldSliderValue;

    public void Start()
    {
        _slider = GetComponentInChildren<Slider>();

        _slider.maxValue = _entityManager.GetLevelSize()[GetPlaneCoordinateIndex(_affectedPlane)];
        _slider.value = _affectedPlane.GetStartPos() + 0.5f;

        
    }


    public void Update()
    {
        oldSliderValue = _slider.value;

        RotateSliderRelativeToCamera();

        //float rotation = GetSliderRotation();

        /*
        if (_affectedPlane.GetPlaneType().Equals(PlaneType.XPLANE))
        {
            rotation -= 90;
        }
        */

        //_slider.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

    public void UpdateSlider(float sliderValue)
    {
        Vector3 desiredPlanePosition = _affectedPlane.transform.position;
        desiredPlanePosition[GetPlaneCoordinateIndex(_affectedPlane)] = sliderValue - 0.5f;

        if (_affectedPlane.MovementValid(desiredPlanePosition))
        {
            _affectedPlane.MovePlane(desiredPlanePosition);
        }
        else
        {
            _slider.value = oldSliderValue;
        }
        
    }

    public void RotateSliderRelativeToCamera()
    {
        Vector3 referenceDirection;
        if (_affectedPlane.GetPlaneType().Equals(PlaneType.XPLANE))
        {
            referenceDirection = Vector3.forward;
        } else
        {
            referenceDirection = Vector3.left;
        }

        Vector3 cameraDirection = new Vector3(_cameraMovement.transform.forward.x, 0, _cameraMovement.transform.forward.z);

        float angle = Vector3.SignedAngle(referenceDirection, cameraDirection, Vector3.up);
        _slider.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    

    
}
