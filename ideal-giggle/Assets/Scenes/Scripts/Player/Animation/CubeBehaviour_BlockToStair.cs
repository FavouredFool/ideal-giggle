using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour_BlockToStair : MonoBehaviour, ICubeBehaviour
{

    [SerializeField]
    private float _cubeLength = 0.5f;

    [SerializeField]
    private float _rollSpeed = 3f;

    private Vector3 _direction;
    private Vector3 _anchor;
    private Vector3 _axis;

    public IEnumerator MoveCubeVisual(Vector3 fromPosition, Vector3 toPosition)
    {
        _direction = (toPosition - fromPosition).normalized;

        if (_direction.y > 0)
        {
            yield return Rotate(180, Vector3.down);

            yield return Rotate(90, Vector3.down);

        } else
        {
            yield return Rotate(90, Vector3.down);

            yield return Rotate(180, Vector3.down);
        }

        
    }

    public IEnumerator Rotate(float angle, Vector3 anchorAngle)
    {
        _anchor = transform.position + (anchorAngle + _direction) * _cubeLength / 2f;
        _axis = Vector3.Cross(Vector3.up, _direction);

        float remainingAngle = angle;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * _rollSpeed * 100, remainingAngle);
            transform.RotateAround(_anchor, _axis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
    }
}
