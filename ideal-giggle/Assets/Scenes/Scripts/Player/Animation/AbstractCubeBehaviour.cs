using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCubeBehaviour : MonoBehaviour
{
    protected bool _isRevered = false;

    public abstract IEnumerator MoveCubeVisual(Vector3 fromPosition, Vector3 toPosition);

    public bool GetIsRevered()
    {
        return _isRevered;
    }

    public void SetIsRevered(bool isRevered)
    {
        _isRevered = isRevered;
    }
}
