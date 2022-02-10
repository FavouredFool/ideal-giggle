using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathCalculator : MonoBehaviour
{

    public int _radius = 5;

    private List<AbstractEntityController> _newList;

    private AbstractEntityController _startEntity;
    private AbstractEntityController _endEntity;

    private int _counter = 0;

    public void Start()
    {
        _newList = new List<AbstractEntityController>();
        
    }

    public List<AbstractEntityController> CalculatePath(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        _counter = 0;
        _startEntity = startEntity;
        _endEntity = endEntity;

        _newList = new List<AbstractEntityController>();

        // Try to get from StartEntity to EndEntity through references
        
        _newList = ReferenceNextEntity(startEntity, new List<AbstractEntityController>());

        /*
        if (_newList != null)
        {
            foreach (AbstractEntityController pathEntity in _newList)
            {
                Debug.Log($"EntityCoordinate in Path: {pathEntity.GetPosition()}");
            }
        } else
        {
            Debug.Log("Path is null");
        }
        */

        Debug.Log($"COUNTER: {_counter}");
        

        return _newList;
    }

    private List<AbstractEntityController> ReferenceNextEntity(AbstractEntityController activeEntity, List<AbstractEntityController> seenEntities)
    {
        _counter++;
        AbstractEntityController referenceEntity;
        List<AbstractEntityController> bestPath = null;
        Vector3 posDifference;

        seenEntities.Add(activeEntity);

        if (activeEntity.Equals(_endEntity))
        {
            return seenEntities;
        }

        for (int i = 0; i < 4; i++)
        {
            referenceEntity = activeEntity.GetEntityReferences()[i];

            if (!referenceEntity)
            {
                continue;
            }

            posDifference = _startEntity.GetPosition() - referenceEntity.GetPosition();
            if (Mathf.Abs(posDifference.x) > _radius || Mathf.Abs(posDifference.y) > _radius || Mathf.Abs(posDifference.z) > _radius)
            {
                continue;
            }

            if (seenEntities.Contains(referenceEntity))
            {
                continue;
            }

            
            var seenEntitiesCopy = new List<AbstractEntityController>(seenEntities);
            List<AbstractEntityController> returnedPath = ReferenceNextEntity(referenceEntity, seenEntitiesCopy);

            if (returnedPath != null)
            {
                if (bestPath == null)
                {
                    bestPath = returnedPath;
                } else if (bestPath.Count > returnedPath.Count)
                {
                    bestPath = returnedPath;
                }
            }
        }

        // Go back out without sending anything back
        return bestPath;
    }
}
