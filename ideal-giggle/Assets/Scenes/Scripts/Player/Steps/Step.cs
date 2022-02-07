using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class Step
{
    private string _animationName;

    private Vector3 _stepGoal;

    private EntityType _entityTypeFrom;

    private EntityType _entityTypeTo;

    public void SetStepGoal(Vector3 stepGoal)
    {
        _stepGoal = stepGoal;
    }

    public Vector3 GetStepGoal()
    {
        return _stepGoal;
    }

    public string GetAnimationName()
    {
        return _animationName;
    }

    public void SetAnimationName(string animationName)
    {
        _animationName = animationName;
    }

}
