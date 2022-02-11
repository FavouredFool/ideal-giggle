using System.Collections;
using UnityEngine;

public class CubeBehaviour_StairToStairUneven : AbstractCubeBehaviour
{
    public override IEnumerator SpecificRotations()
    {
        if (!_isRevered)
        {
            yield return Rotate(180, Vector3.up);
            yield return Rotate(180, Vector3.up);
        }
        else
        {
            yield return Rotate(180, Vector3.down);
            yield return Rotate(180, Vector3.down);
        }
    }
}
