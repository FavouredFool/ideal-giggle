using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EntityHelper;
using static CheckHelper;
using static TWODHelper;
using static ViewHelper;
using static PlaneHelper;


public class PlayerToFrontCalculator : MonoBehaviour
{

    [Header("Dependencies")]
    
    [SerializeField]
    private EntityManager _entityManager;

    [SerializeField]
    private PlaneController _xPlane;

    [SerializeField]
    private PlaneController _zPlane;


    AbstractEntityController _groundEntity;
    private PlayerMovementController _playerMovementController;


    public void Start()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
    }

    public void MovePlayerToFront(bool relative)
    {
        if (!_playerMovementController.GetGroundEntity().GetEntityType2D().Equals(_playerMovementController.GetEntityPositionRelation()))
        {
            return;
        }

        _groundEntity = _playerMovementController.GetGroundEntity();

        PlaneController plane = GetViewPlane(_xPlane, _zPlane);

        if (!PlayerInFrontOfPlane(_playerMovementController, plane))
        {
            return;
        }

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

        }
        else if (entity.GetEntityType2D().Equals(EntityType.STAIR))
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
                if (entityList.Cast<StairController>().Any(stair => StairRotatedInDirection(stair.GetBottomEnter(), GetViewDirectionNormalized(ActiveViewState))))
                {
                    return entity;
                }

                if (entityList.Cast<StairController>().Any(stair => StairRotatedInDirection(stair.GetTopEnter(), GetViewDirectionNormalized(ActiveViewState))))
                {
                    return entity;
                }
            }
            return entityList.FirstOrDefault();
        }
        return null;
    }

}
