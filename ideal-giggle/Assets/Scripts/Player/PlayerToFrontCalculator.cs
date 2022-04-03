using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static CheckHelper;
using static TWODHelper;
using static ViewHelper;


public class PlayerToFrontCalculator : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private PlayerMovementController _playerMovementController;

    [SerializeField]
    private EntityManager _entityManager;

    AbstractEntityController _groundEntity;



    public void MovePlayerToFront(bool relative)
    {
        if (!ViewDimension.Dimension.Equals(Dimension.THREE))
        {
            if (!_playerMovementController.GetGroundEntity().GetEntityType2D().Equals(_playerMovementController.GetEntityPositionRelation()))
            {
                return;
            }
        }

        _groundEntity = _playerMovementController.GetGroundEntity();

        AbstractEntityController entity;

        if (relative)
        {
            entity = GetFrontEntityRelative(_groundEntity);
        }
        else
        {
            entity = GetFrontEntityAbsolute(_groundEntity);
        }

        

        _playerMovementController.MovePlayerToEntity(entity);
    }

    public AbstractEntityController GetFrontEntityAbsolute(AbstractEntityController entity)
    {
        List<AbstractEntityController> entityList = GetEntityListFromPos2D(_entityManager.GetEntityList(), entity.GetPosition());

        if (entity.GetEntityType2D().Equals(EntityType.BLOCK))
        {
            if (EntityExistsInList(_entityManager.GetEntityList(), _groundEntity.GetPosition() + Vector3.up))
            {
                return entity;
            }

        } else if (entity.GetEntityType2D().Equals(EntityType.STAIR))
        {
            if (entityList.Any(e => e.GetEntityType2D().Equals(EntityType.BLOCK)))
            {
                return entity;
            }
        }

        return entityList.FirstOrDefault();
    }


    public AbstractEntityController GetFrontEntityRelative(AbstractEntityController entity)
    {
        List<AbstractEntityController> entityList = GetEntityListFromPos2D(_entityManager.GetEntityList(), entity.GetPosition());

        if (entity.GetEntityType2D().Equals(EntityType.BLOCK))
        {
            if (EntityExistsInList(_entityManager.GetEntityList(), _groundEntity.GetPosition() + Vector3.up))
            {
                return entity;
            }

            return entityList.Where(e => e.GetEntityType().Equals(EntityType.BLOCK)).FirstOrDefault();
        }
        else if (entity.GetEntityType2D().Equals(EntityType.STAIR))
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
