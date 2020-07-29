using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [HideInInspector]
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.aspect = 1.0f/2.0f;
    }

}
