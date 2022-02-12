using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    private PlayerMovementController _playerMovement;


    public void Start()
    {
        _playerMovement = gameObject.GetComponent<PlayerMovementController>();
    }

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

            AbstractEntityController hitEntity = _entityManager.GetEntityFromCoordiantes(hit);
            if (!hitEntity)
            {
                throw new System.Exception();
            }
            _playerMovement.SetEndEntity(hitEntity);
        }
    }

}




