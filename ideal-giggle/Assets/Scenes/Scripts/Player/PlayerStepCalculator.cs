using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;


public class PlayerStepCalculator : MonoBehaviour
{

    [SerializeField]
    private EntityManager _entityManager;

    private StepGoalCalculator _stepGoalCalculator;
    private CubeBehaviourCalculator _cubeBehaviourCalculator;


    public void Start()
    {
        _stepGoalCalculator = GetComponent<StepGoalCalculator>();
        _cubeBehaviourCalculator = GetComponent<CubeBehaviourCalculator>();
    }

    public Step CalculateStep(Vector3 activePosition, Vector3 endPosition)
    {
        var activeStep = new Step();

        AbstractEntityController activeEntity = _entityManager.GetEntityFromCoordiantes(activePosition + Vector3.down);
        AbstractEntityController endEntity = _entityManager.GetEntityFromCoordiantes(endPosition + Vector3.down); ;

        // Calculate Stepgoal
        AbstractEntityController stepGoal;
        stepGoal = _stepGoalCalculator.CalculateStepGoalEntity(activeEntity, endEntity);
        activeStep.SetStepGoalEntity(stepGoal);

        // Calculate Animation to use
        AbstractCubeBehaviour cubeBehaviour;
        cubeBehaviour = _cubeBehaviourCalculator.CalculateCubeBehaviour(activeEntity, stepGoal);
        activeStep.SetCubeBehaviour(cubeBehaviour);

        return activeStep;
    }

}



