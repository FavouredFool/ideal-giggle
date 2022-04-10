using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewHelper;

public class CameraAnimation : MonoBehaviour
{

    Vector3 _pivot;

    [SerializeField]
    private float _speed;

    private CameraMovement _cameraMovement;

    public void Start()
    {
        _cameraMovement = GetComponent<CameraMovement>();
    }

    public IEnumerator AnimateCamera(Vector3 pivot, int hDegrees, int vDegrees, ViewHelper.ViewState desiredViewState)
    {
        _pivot = pivot;

        yield return StartAnimation(desiredViewState, hDegrees, vDegrees);
    }

    public IEnumerator StartAnimation(ViewState desiredViewState, int hDegrees, int vDegrees)
    {
        yield return null;
        //transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), -vDegrees);
        //transform.RotateAround(_pivot, Vector3.up, hDegrees);

        
        Vector3 startPosition = transform.localPosition;
        Vector3 startPositionNormalized = startPosition.normalized;
        
        Vector3 viewDirection = GetViewDirectionNormalized(desiredViewState);

        Quaternion rotationForGoalPosition = Quaternion.FromToRotation(startPositionNormalized.normalized - Vector3.zero, -viewDirection);
        Vector3 newPosition = rotationForGoalPosition * startPosition;

        Quaternion startRotation = transform.localRotation;
        Quaternion goalRotation = Quaternion.LookRotation(-newPosition.normalized);


        float t = 0;
        while (t < 1)
        {
            t += _speed * Time.deltaTime;

            Quaternion slerpedRotationForGoalPosition = Quaternion.Slerp(Quaternion.identity, rotationForGoalPosition, t);
            Vector3 positionWithSlerpedRotation = slerpedRotationForGoalPosition * startPosition;
            transform.localPosition = positionWithSlerpedRotation;

            Quaternion slerpedRotation = Quaternion.Slerp(startRotation, goalRotation, t);
            transform.localRotation = slerpedRotation;
            yield return null;
        }

        transform.localPosition = newPosition;
        transform.localRotation = Quaternion.LookRotation(-newPosition.normalized);
        

    }

}
