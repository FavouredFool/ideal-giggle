using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    public void MovePlane(Vector3 direction)
    {
        Vector3 desiredPosition = transform.position + direction;

        bool xGuard = desiredPosition.x < -0.5f || desiredPosition.x > _entityManager.GetLevelSize().x - 0.5f;
        bool zGuard = desiredPosition.z < -0.5f || desiredPosition.z > _entityManager.GetLevelSize().z - 0.5f;

        if (xGuard || zGuard)
        {
            return;
        }

        transform.localPosition = desiredPosition;
        _entityManager.UpdateColor();
        _entityManager.UpdateReferences();

    }

}
