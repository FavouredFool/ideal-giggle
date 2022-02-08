using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeBehaviour
{
    IEnumerator MoveCubeVisual(Vector3 fromPosition, Vector3 toPosition);
}
