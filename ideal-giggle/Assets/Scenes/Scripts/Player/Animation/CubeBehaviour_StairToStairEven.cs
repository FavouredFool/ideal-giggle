using System.Collections;
using UnityEngine;

public class CubeBehaviour_StairToStairEven : AbstractCubeBehaviour
{
    public override IEnumerator SpecificRotations()
    {
        yield return Rotate(180, Vector3.up);
        yield return Rotate(180, Vector3.down);
    }
}
