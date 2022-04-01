using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class PlayerToFrontCalculator : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private PlayerMovementController _playerMovementController;

    [SerializeField]
    private EntityManager _entityManager;

    AbstractEntityController _groundEntity;



    public void MovePlayerToFront()
    {
        _groundEntity = _playerMovementController.GetGroundEntity();

        if (_groundEntity.GetEntityType().Equals(EntityType.BLOCK))
        {
            if (_entityManager.GuardPlayerToFront(_groundEntity.GetPosition()))
            {
                return;
            }
        }

        AbstractEntityController entity = null;
        if (_groundEntity.GetEntityType().Equals(EntityType.BLOCK))
        {
            entity = _entityManager.GetFrontEntity(_groundEntity, EntityType.BLOCK);
        }
        else if (_groundEntity.GetEntityType().Equals(EntityType.STAIR))
        {
            entity = _entityManager.GetFrontEntity(_groundEntity, EntityType.STAIR);
        }
        else
        {
            Debug.LogWarning("FEHLER");
        }


        if (!_groundEntity.GetEntityType().Equals(entity.GetEntityType()))
        {
            return;
        }

        // Guard, dass vor / hinter Stairs keine Blocks mehr sind

        // Guard, dass Stairs richtig stehen -> Orthogonal zur Kamera

        _playerMovementController.MovePlayerToEntity(entity);
    }

}
