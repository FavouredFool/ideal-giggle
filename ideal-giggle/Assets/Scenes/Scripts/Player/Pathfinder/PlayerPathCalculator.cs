using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPathCalculator : MonoBehaviour
{

    public int _radius = 5;
    public Material pathFinalMaterial;
    public Material pathCheckMaterial;
    public Material startMaterial;
    public Material pathCheckedMaterial;

    [SerializeField]
    private EntityManager _entityManager;


    public IEnumerator CalculatePathAstar(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        foreach (AbstractEntityController entity in _entityManager.GetEntityList())
        {
            entity.ChangeMaterial(startMaterial);
            entity.SetG(Vector3.Distance(entity.GetPosition(), startEntity.GetPosition()));
            entity.SetH(Vector3.Distance(entity.GetPosition(), endEntity.GetPosition()));
        }

        var toSearch = new List<AbstractEntityController>() { startEntity };
        var processed = new List<AbstractEntityController>();

        while(toSearch.Any())
        {
            yield return null;
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

            current.ChangeMaterial(pathCheckedMaterial);

            if (current.Equals(endEntity))
            {
                AbstractEntityController currentPathTile = endEntity;
                var path = new List<AbstractEntityController>();
                var loopCount = 1000;

                while (currentPathTile != startEntity)
                {
                    path.Add(currentPathTile);
                    currentPathTile.ChangeMaterial(pathFinalMaterial);
                    currentPathTile = currentPathTile.Connection;
                    loopCount--;
                    if (loopCount < 0) throw new System.Exception();

                    yield return new WaitForSeconds(0.05f);
                }

                path.Add(currentPathTile);
                path.Reverse();

                yield return path;
                yield break;
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
                        neighbor.SetH(Vector3.Distance(neighbor.GetPosition(), endEntity.GetPosition()));
                        toSearch.Add(neighbor);
                        neighbor.ChangeMaterial(pathCheckMaterial);
                    }
                }
            }
        }
    }

}
