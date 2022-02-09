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

        // Calculate Stepgoal
        Vector3 stepGoal;
        stepGoal = _stepGoalCalculator.CalculateStepGoal(activePosition, endPosition);
        activeStep.SetStepGoal(stepGoal);


        // Calculate Animation to use
        ICubeBehaviour cubeBehaviour;
        cubeBehaviour = _cubeBehaviourCalculator.CalculateCubeBehaviour(activePosition, stepGoal);
        activeStep.SetCubeBehaviour(cubeBehaviour);

        return activeStep;
    }

}



