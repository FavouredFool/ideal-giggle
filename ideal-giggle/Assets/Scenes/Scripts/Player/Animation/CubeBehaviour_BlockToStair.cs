using System.Collections;
using UnityEngine;

public class CubeBehaviour_BlockToStair : AbstractCubeBehaviour
{

    public override IEnumerator SpecificRotations()
    {
        if (!_isReversed)
        {
            yield return Rotate(90, Vector3.down);
            yield return Rotate(180, Vector3.down);
        } else
        {
            yield return Rotate(90, Vector3.down);
            yield return Rotate(180, Vector3.up);
        }
    }

}
