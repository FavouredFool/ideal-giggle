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
            hit = CalculateMovePosFromClickPos(hit);

            AbstractEntityController hitEntity = _entityManager.GetEntityFromCoordiantes(hit);
            if (!hitEntity)
            {
                throw new System.Exception();
            }
            _playerMovement.SetEndEntity(hitEntity);
        }
    }


    public Vector3 CalculateMovePosFromClickPos(Vector3 hit)
    {
        // When you click the side of a square, the click should register at the top of the whole square-stack
        var differentY = new List<Vector3>();

        foreach (AbstractEntityController entity in _entityManager.GetEntityList())
        {
            if (entity.GetPosition().x.Equals(hit.x) && entity.GetPosition().z.Equals(hit.z))
            {
                differentY.Add(entity.GetPosition());
            }
        }

        for (int i = 0; i < differentY.Count; i++)
        {
            if (!differentY.Contains(hit))
            {
                break;
            }
        }

        return hit;
    }

    

}



