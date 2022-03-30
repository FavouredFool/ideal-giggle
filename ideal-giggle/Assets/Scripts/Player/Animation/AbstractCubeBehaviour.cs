using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbstractCubeBehaviour : MonoBehaviour
{

    [SerializeField]
    private float _rollSpeed = 3f;

    protected float _cubeLength = 0.5f;
    protected bool _isReversed = false;
    protected Vector3 _direction;
   
    public IEnumerator MoveCubeVisual(Vector3 direction)
    {
        _direction = direction;
        yield return SpecificRotations();
    }

    public abstract IEnumerator SpecificRotations();

    public IEnumerator Rotate(float angle, Vector3 anchorAngle)
    {
        Vector3 anchor = transform.position + (anchorAngle + _direction) * _cubeLength / 2f;
        Vector3 axis = Vector3.Cross(Vector3.up, _direction);

        float remainingAngle = angle;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * _rollSpeed * 100, remainingAngle);
            transform.RotateAround(anchor, axis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        // Correct Rounding Errors manually
        float xCorrected = (float)Math.Round(transform.localPosition.x, 2);
        float yCorrected = (float)Math.Round(transform.localPosition.y, 2);
        float zCorrected = (float)Math.Round(transform.localPosition.z, 2);

        transform.localPosition = new Vector3(xCorrected, yCorrected, zCorrected);
    }

    public bool GetIsReversed()
    {
        return _isReversed;
    }

    public void SetIsReversed(bool isReversed)
    {
        _isReversed = isReversed;
    }

    
}
