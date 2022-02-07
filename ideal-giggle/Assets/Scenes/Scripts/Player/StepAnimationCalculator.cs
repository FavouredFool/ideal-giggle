using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAnimationCalculator : MonoBehaviour
{

    [SerializeField]
    private EntityManager _entityManager;

    public string CalculateAnimation(Vector3 activePosition, Vector3 stepGoal)
    {
        EntityController entityFrom = _entityManager.GetEntityFromCoordiantes(activePosition + Vector3.down);
        EntityController entityTo = _entityManager.GetEntityFromCoordiantes(stepGoal + Vector3.down);

        string animationName = "StraightStep";

        if (entityFrom)
        {

        }
        else
        {

        }

        if (entityTo)
        {
        }
        else
        {


        }

        return animationName;
    }


}
