using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPathCalculator : MonoBehaviour
{

    public int _radius = 5;
    public Material pathMaterial;

    [SerializeField]
    private EntityManager _entityManager;

    private List<AbstractEntityController> _newList;

    private AbstractEntityController _startEntity;
    private AbstractEntityController _endEntity;

    private int _counter = 0;

    public void Start()
    {
        _newList = new List<AbstractEntityController>();
        
    }

    public List<AbstractEntityController> CalculatePathAstar(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        foreach (AbstractEntityController entity in _entityManager.GetEntityList())
        {
            entity.SetG(Vector3.Distance(entity.GetPosition(), startEntity.GetPosition()));
            entity.SetH(Vector3.Distance(entity.GetPosition(), endEntity.GetPosition()));
        }

        var toSearch = new List<AbstractEntityController>() { startEntity };
        var processed = new List<AbstractEntityController>();

        while(toSearch.Any())
        {
            AbstractEntityController current = toSearch[0];

            foreach(AbstractEntityController t in toSearch)
            {
                if (t.F <= current.F && t.H < current.H)
                {
                    current = t;
                }
            }

            processed.Add(current);
            toSearch.Remove(current);

            if (current.Equals(endEntity))
            {
                AbstractEntityController currentPathTile = endEntity;
                var path = new List<AbstractEntityController>();
                var loopCount = 1000;

                while (currentPathTile != startEntity)
                {
                    Debug.Log(currentPathTile.Connection);
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    loopCount--;
                    if (loopCount < 0) throw new System.Exception();
                }

                path.Add(currentPathTile);
                path.Reverse();

                return path;
            }

            foreach (var neighbor in current.GetEntityReferences())
            {
                if (neighbor == null)
                {
                    continue;
                }
                if (processed.Contains(neighbor))
                {
                    continue;
                }

                bool inSearch = toSearch.Contains(neighbor);

                float costToNeighbor = current.G + Vector3.Distance(current.GetPosition(), neighbor.GetPosition());

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetH(Vector3.Distance(neighbor.GetPosition(), startEntity.GetPosition()));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }



    public List<AbstractEntityController> CalculatePathBad(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        _counter = 0;
        _startEntity = startEntity;
        _endEntity = endEntity;

        _newList = new List<AbstractEntityController>();

        // Try to get from StartEntity to EndEntity through references
        
        _newList = ReferenceNextEntity(startEntity, new List<AbstractEntityController>());

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
