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

    public IEnumerator AnimateCamera(Vector3 pivot, ViewState desiredViewState)
    {
        _pivot = pivot;

        yield return StartAnimation(desiredViewState);
    }

    public IEnumerator StartAnimation(ViewState desiredViewState)
    {
        Vector3 startPosition = transform.localPosition - _pivot;
        Vector3 startDirectionNormalized = startPosition.normalized;
        
        Vector3 viewDirection = GetViewDirectionNormalized(desiredViewState);
        Quaternion rotationForGoalPosition = Quaternion.FromToRotation(startDirectionNormalized.normalized, -viewDirection);

        float t = 0;
        while (t < 1)
        {
            t += _speed * Time.deltaTime;

            Quaternion slerpedRotationForGoalPosition = Quaternion.Slerp(Quaternion.identity, rotationForGoalPosition, t);
            Vector3 positionWithSlerpedRotation = slerpedRotationForGoalPosition * startPosition;

            transform.localPosition = positionWithSlerpedRotation + _pivot;
            transform.localRotation = Quaternion.LookRotation(-positionWithSlerpedRotation);

            yield return null;
        }

    }

}
