using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        //  float screenAspect = 1280 / 720;  现在android手机的主流分辨。
        //  mainCamera.aspect --->  摄像机的长宽比（宽度除以高度）
        camera.aspect = 1.0f/2.0f;
    }

}
