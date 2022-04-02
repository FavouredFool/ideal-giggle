using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class BlockController : AbstractEntityController
{
    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.BLOCK;
        _entityType2D = EntityType.BLOCK;
        _visualPosition = new Vector3(0, -0.25f, 0);
    }
}

