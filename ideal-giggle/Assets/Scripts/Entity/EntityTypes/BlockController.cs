using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class BlockController : AbstractEntityController
{
    BlockReferenceController _blockReferenceController;
    

    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.BLOCK;
        _visualPosition = new Vector3(0, -0.25f, 0);

        _blockReferenceController = GetComponent<BlockReferenceController>();
    }

    public override List<AbstractEntityController> CalculateReferences3D()
    {
        return _blockReferenceController.CalculateReferences3D(_entityCache, _position);
    }

    public override List<AbstractEntityController> CalculateReferences2D(PlaneController xPlane, PlaneController zPlane)
    {
        return _blockReferenceController.CalculateReferences2D(xPlane, zPlane);
    }

}

