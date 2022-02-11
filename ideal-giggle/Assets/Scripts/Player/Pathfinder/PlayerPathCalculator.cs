using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPathCalculator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    [Header("Materials")]
    [SerializeField]
    private Material pathFinalMaterial;

    [SerializeField]
    private Material pathCheckMaterial;

    [SerializeField]
    private Material pathCheckedMaterial;

    private Material defaultMaterial;

    [Header("Visualisation")]
    [SerializeField]
    bool _visualise = true;

    [SerializeField]
    float _createPathTimeInSeconds = 0.05f;

    AbstractEntityController _startEntity;
    AbstractEntityController _endEntity;

    List<AbstractEntityController> _toSearchList;
    List<AbstractEntityController> _processedList;


    public void Start()
    {
        defaultMaterial = _entityManager.GetEntityMaterial();
    }


    public IEnumerator CalculatePathAstar(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        _startEntity = startEntity;
        _endEntity = endEntity;

        _toSearchList = new List<AbstractEntityController>() { _startEntity };
        _processedList = new List<AbstractEntityController>();

        foreach (AbstractEntityController entity in _entityManager.GetEntityList())
        {
            entity.SetG(Vector3.Distance(entity.GetPosition(), startEntity.GetPosition()));
            entity.SetH(Vector3.Distance(entity.GetPosition(), endEntity.GetPosition()));
        }

        while(_toSearchList.Any())
        {
            AbstractEntityController current;
            current = CalculateCurrentEntity(_toSearchList);

            _processedList.Add(current);
            _toSearchList.Remove(current);

            if(_visualise)
            {
                current.ChangeMaterial(pathCheckedMaterial);
                yield return null;
            }
            
            if (current.Equals(endEntity))
            {
                AbstractEntityController currentPathTile = _endEntity;
                var path = new List<AbstractEntityController>();
                var loopCount = 1000;

                while (currentPathTile != _startEntity)
                {
                    path.Add(currentPathTile);

                    if (_visualise)
                    {
                        currentPathTile.ChangeMaterial(pathFinalMaterial);
                        yield return new WaitForSeconds(_createPathTimeInSeconds);
                    }

                    currentPathTile = currentPathTile.Connection;
                    loopCount--;
                    if (loopCount < 0)
                    {
                        throw new System.Exception();
                    }
                }

                path.Add(currentPathTile);
                path.Reverse();

                if (_visualise)
                {
                    currentPathTile.ChangeMaterial(pathFinalMaterial);
                    yield return new WaitForSeconds(_createPathTimeInSeconds);
                }

                yield return path;

                RevertMaterial(defaultMaterial);
                yield break;
            }

            foreach (AbstractEntityController neighbor in current.GetEntityReferences())
            {
                CalculateIfEntityIsAddedToToSearch(current, neighbor);
            }

        }

        RevertMaterial(defaultMaterial);
        
    }


    private AbstractEntityController CalculateCurrentEntity(List<AbstractEntityController> toSearch)
    {
        AbstractEntityController current = toSearch[0];

        foreach (AbstractEntityController t in toSearch)
        {
            if (t.F <= current.F && t.H < current.H)
            {
                current = t;
            }
        }
        return current;
    }

    private void CalculateIfEntityIsAddedToToSearch(AbstractEntityController current, AbstractEntityController neighbor)
    {
        if (neighbor == null)
        {
            return;
        }
        if (_processedList.Contains(neighbor))
        {
            return;
        }

        bool inSearch = _toSearchList.Contains(neighbor);

        float costToNeighbor = current.G + Vector3.Distance(current.GetPosition(), neighbor.GetPosition());

        if (!inSearch || costToNeighbor < neighbor.G)
        {
            neighbor.SetG(costToNeighbor);
            neighbor.SetConnection(current);

            if (!inSearch)
            {
                neighbor.SetH(Vector3.Distance(neighbor.GetPosition(), _endEntity.GetPosition()));
                _toSearchList.Add(neighbor);

                if (_visualise)
                {
                    neighbor.ChangeMaterial(pathCheckMaterial);
                }

            }
        }
        
    }

    private void RevertMaterial(Material defaultMaterial)
    {
        if (_visualise)
        {
            foreach (AbstractEntityController entity in _entityManager.GetEntityList())
            {
                entity.ChangeMaterial(defaultMaterial);
            }
        }
    }

}
