using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightStep : AbstractStep
{
    [SerializeField]
    private PlayerMovementController _playerMovement;

    public override void MoveStep()
    {
        Debug.Log($"Position: {transform.position} Movement: {Movement}");
        _playerMovement.transform.position += Movement;
    }
}
