using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step
{
    private ICubeBehaviour _cubeBehaviour;

    private Vector3 _stepGoal;

    public void SetStepGoal(Vector3 stepGoal)
    {
        _stepGoal = stepGoal;
    }

    public Vector3 GetStepGoal()
    {
        return _stepGoal;
    }

    public ICubeBehaviour GetCubeBehavior()
    {
        return _cubeBehaviour;
    }

    public void SetCubeBehaviour(ICubeBehaviour cubeBehaviour)
    {
        _cubeBehaviour = cubeBehaviour;
    }

}
