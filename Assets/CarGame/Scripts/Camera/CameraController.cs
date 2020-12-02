using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public const float BaseAspectRatio = 1600.0f / 900.0f;
    
    /*
     * Adjusts camera's projection matrix so that
     * the level always fit tightly into the scene
     * no matter the resolution
     */
    void Start()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        Camera.main.projectionMatrix = Matrix4x4.Scale(new Vector3(currentAspectRatio / BaseAspectRatio, 1.0f, 1.0f)) * Camera.main.projectionMatrix;
    }
}
