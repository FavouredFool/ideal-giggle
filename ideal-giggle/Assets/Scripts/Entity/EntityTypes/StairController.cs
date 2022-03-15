using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StairController : AbstractEntityController
{

    StairReferenceController _stairReferenceController;

    Vector3 _bottomEnter;
    Vector3 _topEnter;

    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.STAIR;
        _visualPosition = new Vector3(0, -0.75f, 0);
        _stairReferenceController = GetComponent<StairReferenceController>();

        Vector3 rotation = transform.eulerAngles;


        if (rotation.x == 0 && rotation.z == 0)
        {
            switch ((int)(rotation.y / 90) % 4)
            {
                case 0:
                    _bottomEnter = Vector3.left;
                    _topEnter = Vector3.right;
                    break;
                case 1:
                    _bottomEnter = Vector3.forward;
                    _topEnter = Vector3.back;
                    break;
                case 2:
                    _bottomEnter = Vector3.right;
                    _topEnter = Vector3.left;
                    break;
                case 3:
                    _bottomEnter = Vector3.back;
                    _topEnter = Vector3.forward;
                    break;
            }
        }
        else
        {
            _bottomEnter = Vector3.zero;
            _topEnter = Vector3.zero;
        }
    }

    public override List<AbstractEntityController> CalculateReferences3D()
    {
       return _stairReferenceController.CalculateReferences3D(_entityCache, _position);
    }

    public override List<AbstractEntityController> CalculateReferences2D(PlaneController xPlane, PlaneController zPlane)
    {
        return _stairReferenceController.CalculateReferences2D(xPlane, zPlane);
    }

    public Vector3 GetTopEnter()
    {
        return _topEnter;
    }

    public Vector3 GetBottomEnter()
    {
        return _bottomEnter;
    }
}

