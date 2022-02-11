using System.Collections.Generic;
using UnityEngine;


public class PlayerStepCalculator : MonoBehaviour
{
    private StepGoalCalculator _stepGoalCalculator;
    private CubeBehaviourCalculator _cubeBehaviourCalculator;


    public void Start()
    {
        _stepGoalCalculator = GetComponent<StepGoalCalculator>();
        _cubeBehaviourCalculator = GetComponent<CubeBehaviourCalculator>();
    }

    public Step CalculateStep(AbstractEntityController activeEntity, List<AbstractEntityController> playerMovementPath)
    {
        var activeStep = new Step();

        // Calculate Stepgoal
        AbstractEntityController stepGoalEntity;
        stepGoalEntity = _stepGoalCalculator.CalculateStepGoalEntity(activeEntity, playerMovementPath);
        activeStep.SetStepGoalEntity(stepGoalEntity);

        // Calculate Animation to use
        AbstractCubeBehaviour cubeBehaviour;
        cubeBehaviour = _cubeBehaviourCalculator.CalculateCubeBehaviour(activeEntity, stepGoalEntity);
        activeStep.SetCubeBehaviour(cubeBehaviour);

        return activeStep;
    }

}



