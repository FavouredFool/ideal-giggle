using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static CheckHelper;
using static TWODHelper;


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
            if (EntityExistsInList(_entityManager.GetEntityList(), _groundEntity.GetPosition() + Vector3.up))
            {
                return;
            }
        }

        AbstractEntityController entity = GetFrontEntity(_groundEntity, _groundEntity.GetEntityType());

        _playerMovementController.MovePlayerToEntity(entity);
    }

    public AbstractEntityController GetFrontEntity(AbstractEntityController entity, EntityType searchedType)
    {
        List<AbstractEntityController> entityList = GetEntityListFromPos2D(_entityManager.GetEntityList(), entity.GetPosition());

        if (searchedType.Equals(EntityType.BLOCK))
        {
            return entityList.Where(e => e.GetEntityType().Equals(EntityType.BLOCK)).FirstOrDefault();
        }
        else if (searchedType.Equals(EntityType.STAIR))
        {
            if (entityList.Any(e => e.GetEntityType().Equals(EntityType.BLOCK)))
            {
                return entity;
            }
            else
            {
                if (entityList.Cast<StairController>().Any(stair => StairRotatedInDirection(stair.GetBottomEnter(), GetViewDirection())))
                {
                    return entity;
                }

                if (entityList.Cast<StairController>().Any(stair => StairRotatedInDirection(stair.GetTopEnter(), GetViewDirection())))
                {
                    return entity;
                }
            }
            return entityList.FirstOrDefault();
        }
        return null;
    }

}
