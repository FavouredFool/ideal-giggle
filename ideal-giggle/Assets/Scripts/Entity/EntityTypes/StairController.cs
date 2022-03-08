using UnityEngine;
using static EntityHelper;

public class StairController : AbstractEntityController
{

    bool _transitionIsSet = false;
    Vector3 _referenceDirection;

    Vector3 _bottomEnter;
    Vector3 _topEnter;

    public override void Awake()
    {
        base.Awake();
        _entityType = EntityType.STAIR;
        _visualPosition = new Vector3(0, -0.75f, 0);

        // Vector3's um aufzuzeigen wie eine Treppe betreten werden kann
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
        } else
        {
            _bottomEnter = Vector3.zero;
            _topEnter = Vector3.zero;
        }

        // Debug.Log($"BottomEnter: {_bottomEnter}, TopEnter: {_topEnter}");

    }

    public override void CalculateReferences()
    {

        for (int i = 0; i < _entityReferences.Count; i++)
        {
            _entityReferences[i] = null;
        }

        for (int i = 0; i < 4; i++)
        {
            _referenceDirection = SwitchReferenceDirection(i);
            _transitionIsSet = false;

            

            EvaluateUpperRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateMiddleRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            EvaluateLowerRow(i);
            if (_transitionIsSet)
            {
                continue;
            }

            _entityReferences[i] = null;
        }
    }

    public Vector3 SwitchReferenceDirection(int index)
    {
        Vector3 referenceDirection;

        switch (index)
        {
            case 0:
                referenceDirection = Vector3.forward;
                break;
            case 1:
                referenceDirection = Vector3.right;
                break;
            case 2:
                referenceDirection = Vector3.back;
                break;
            case 3:
                referenceDirection = Vector3.left;
                break;
            default:
                referenceDirection = Vector3.zero;
                Debug.LogWarning($"FEHLER: referenceDirection darf niemals {referenceDirection} sein.");
                break;
        }

        return referenceDirection;
    }

    public void EvaluateUpperRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {

            if (!(activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.up)))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(-GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    // Check ob obere Treppe bedeckt ist, oder über dem Spieler ein Block ist

                    bool foundCeil = false;
                    foreach (AbstractEntityController ceilingEntity in _entityCache)
                    {
                        if (ceilingEntity.GetPosition().Equals(_position + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }

                        if (ceilingEntity.GetPosition().Equals(_position + _referenceDirection + (Vector3.up * 2)))
                        {
                            foundCeil = true;
                            break;
                        }
                    }

                    if (foundCeil)
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }

    public void EvaluateMiddleRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!activeEntity.GetPosition().Equals(_position + _referenceDirection))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:

                    if (!_referenceDirection.Equals(-GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(-GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {activeEntity.GetEntityType()} sein");
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
    }

    public void EvaluateLowerRow(int index)
    {
        foreach (AbstractEntityController activeEntity in _entityCache)
        {
            if (!activeEntity.GetPosition().Equals(_position + _referenceDirection + Vector3.down))
            {
                continue;
            }

            _transitionIsSet = true;

            switch (activeEntity.GetEntityType())
            {
                case EntityType.BLOCK:

                    if (!_referenceDirection.Equals(-GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                case EntityType.STAIR:

                    if (!_referenceDirection.Equals(-GetBottomEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    if (!_referenceDirection.Equals(((StairController)activeEntity).GetTopEnter()))
                    {
                        _entityReferences[index] = null;
                        break;
                    }

                    _entityReferences[index] = activeEntity;
                    break;
                default:
                    Debug.LogWarning($"FEHLER: activeEntity.GetEntityType() darf nicht {activeEntity.GetEntityType()} sein");
                    _entityReferences[index] = null;
                    break;
            }

            break;
        }
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