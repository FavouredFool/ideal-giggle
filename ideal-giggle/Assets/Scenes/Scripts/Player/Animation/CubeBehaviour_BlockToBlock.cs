using System.Collections;
using UnityEngine;

public class CubeBehaviour_BlockToBlock : AbstractCubeBehaviour
{
    public override IEnumerator SpecificRotations()
    {
        yield return Rotate(90, Vector3.down);
        yield return Rotate(90, Vector3.down);
    }
}
