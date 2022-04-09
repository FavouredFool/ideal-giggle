using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField]
    private PlayerMovementController _playerMovement;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 hit;
            hit = RayHelper.CastRayFromMousePosition();

            if (hit.Equals(Vector3.negativeInfinity))
            {
                return;
            }

            hit = CoordinateHelper.DetermineGridCoordinate(hit);

            _playerMovement.InterpretHitInput(hit);
            
        }
    }

}




