using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;


public class PlayerStepCalculator : MonoBehaviour
{

    [SerializeField]
    private EntityManager _entityManager;

    private StepGoalCalculator _stepGoalCalculator;
    private StepAnimationCalculator _stepAnimationCalculator;


    public void Start()
    {
        _stepGoalCalculator = GetComponent<StepGoalCalculator>();
        _stepAnimationCalculator = GetComponent<StepAnimationCalculator>();
    }

    public Step CalculateStep(Vector3 activePosition, Vector3 endPosition)
    {
        var activeStep = new Step();

        // Calculate Stepgoal
        Vector3 stepGoal;
        stepGoal = _stepGoalCalculator.CalculateStepGoal(activePosition, endPosition);
        activeStep.SetStepGoal(stepGoal);


        // Calculate Animation to use
        string animationName;
        animationName = _stepAnimationCalculator.CalculateAnimation(activePosition, stepGoal);
        activeStep.SetAnimationName(animationName);



        return activeStep;
    }

}



