using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour_BlockToStairs : MonoBehaviour, ICubeBehaviour
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
            Debug.Log("higher");

            yield return Rotate90Degrees();

            yield return Rotate90Degrees();

        } else
        {
            Debug.Log("lower");

            yield return Rotate90Degrees();

            yield return Rotate90Degrees();

            yield return Rotate90Degrees();
        }

        
    }

    public IEnumerator Rotate90Degrees()
    {
        
        _anchor = transform.position + (Vector3.down + _direction) * _cubeLength / 2f;
        _axis = Vector3.Cross(Vector3.up, _direction);

        float remainingAngle = 90;

        while(remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * _rollSpeed * 100, remainingAngle);
            transform.RotateAround(_anchor, _axis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
    }
}
