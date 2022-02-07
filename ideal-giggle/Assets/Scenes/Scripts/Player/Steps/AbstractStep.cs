using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractStep : MonoBehaviour
{
    [SerializeField]
    private string _animationName;

    private Vector3 _stepMovement;

    public void SetStepMovement(Vector3 stepMovement)
    {
        _stepMovement = stepMovement;
    }

    public Vector3 GetStepMovement()
    {
        return _stepMovement;
    }

    public string GetAnimationName()
    {
        return _animationName;
    }
}
