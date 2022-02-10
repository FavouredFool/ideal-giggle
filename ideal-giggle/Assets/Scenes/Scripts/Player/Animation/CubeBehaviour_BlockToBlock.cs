using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour_BlockToBlock : AbstractCubeBehaviour
{

    [SerializeField]
    private float _cubeLength = 0.5f;

    [SerializeField]
    private float _rollSpeed = 3f;

    private Vector3 _direction;
    private Vector3 _anchor;
    private Vector3 _axis;

    public override IEnumerator MoveCubeVisual(Vector3 fromPosition, Vector3 toPosition)
    {
        _direction = (toPosition - fromPosition).normalized;

        yield return Rotate(90, Vector3.down);

        yield return Rotate(90, Vector3.down);
    }

    public IEnumerator Rotate(float angle, Vector3 anchorAngle)
    {
        _anchor = transform.position + (anchorAngle + _direction) * _cubeLength / 2f;
        _axis = Vector3.Cross(Vector3.up, _direction);

        float remainingAngle = angle;

        int loopcounter = 0;
        while (remainingAngle > 0 || loopcounter>1000)
        {
            loopcounter++;
            float rotationAngle = Mathf.Min(Time.deltaTime * _rollSpeed * 100, remainingAngle);
            transform.RotateAround(_anchor, _axis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        if (loopcounter>1000)
        {
            Debug.LogWarning("Endlosschleife erzeugt");
        }
    }
}
