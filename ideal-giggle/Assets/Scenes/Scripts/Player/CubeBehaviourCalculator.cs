using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviourCalculator : MonoBehaviour
{

    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlayerVisualController _playerVisual;

    public ICubeBehaviour CalculateCubeBehaviour(Vector3 activePosition, Vector3 stepGoal)
    {
        EntityController entityFrom = _entityManager.GetEntityFromCoordiantes(activePosition + Vector3.down);
        EntityController entityTo = _entityManager.GetEntityFromCoordiantes(stepGoal + Vector3.down);

        ICubeBehaviour cubeBehaviour = _playerVisual.GetComponent<CubeBehaviour_BlockToBlock>();

        return cubeBehaviour;
    }


}
