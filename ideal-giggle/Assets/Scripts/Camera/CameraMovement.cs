using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private EntityManager _entityManager;

    private Vector3 pivot;

    private void Start()
    {
        Vector3 dimensions = _entityManager.GetDimensions();

        pivot = new Vector3((dimensions.x-1) / 2f, (dimensions.y-1) / 2f, (dimensions.z-1) / 2f);
        Debug.Log(pivot);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.RotateAround(pivot, Vector3.up, 45);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.RotateAround(pivot, Vector3.up, -45);
        }
    }
}
