using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step
{
    private AbstractCubeBehaviour _cubeBehaviour;

    private AbstractEntityController _stepGoalEntity;

    public void SetStepGoalEntity(AbstractEntityController stepGoalEntity)
    {
        _stepGoalEntity = stepGoalEntity;
    }

    public AbstractEntityController GetStepGoalEntity()
    {
        return _stepGoalEntity;
    }

    public AbstractCubeBehaviour GetCubeBehavior()
    {
        return _cubeBehaviour;
    }

    public void SetCubeBehaviour(AbstractCubeBehaviour cubeBehaviour)
    {
        _cubeBehaviour = cubeBehaviour;
    }

}
