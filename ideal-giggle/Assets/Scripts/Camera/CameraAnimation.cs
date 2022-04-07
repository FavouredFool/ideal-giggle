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

    public IEnumerator AnimateCamera(Vector3 pivot, int hDegrees, int vDegrees, ViewState desiredViewState)
    {
        _pivot = pivot;

        yield return StartAnimation(desiredViewState, hDegrees, vDegrees);
    }

    public IEnumerator StartAnimation(ViewState desiredViewState, int hDegrees, int vDegrees)
    {
        yield return new WaitForSeconds(0.2f);
        transform.RotateAround(_pivot, Vector3.Cross(transform.forward, Vector3.up), -vDegrees);
        transform.RotateAround(_pivot, Vector3.up, hDegrees);
    }

}
