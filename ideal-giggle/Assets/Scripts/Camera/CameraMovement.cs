using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.RotateAround(new Vector3(4, 0, 4), Vector3.up, 45);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.RotateAround(new Vector3(4, 0, 4), Vector3.up, -45);
        }
    }
}
