using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChanger : MonoBehaviour
{
    Camera thisCamera;
    public Vector3 resolution = new Vector3(1920f, 1080f);
    void Update()
    {
        thisCamera = GetComponent<Camera>();
        Vector2 resTarget = resolution;
        Vector2 resViewport = new Vector2(Screen.width, Screen.height);
        Vector2 resNormalized = resTarget / resViewport;
        Vector2 size = resNormalized / Mathf.Max(resNormalized.x, resNormalized.y);
        thisCamera.rect = new Rect(default, size) { center = new Vector2(0.5f, 0.5f) };
    }
}
