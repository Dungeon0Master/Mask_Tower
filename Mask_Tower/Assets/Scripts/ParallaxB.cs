using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxMultiplier = 0.3f;

    private Transform cam;
    private Vector3 lastCamPosition;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
    }

    void LateUpdate()
    {
        float deltaX = cam.position.x - lastCamPosition.x;
        transform.position += new Vector3(
            deltaX * parallaxMultiplier,
            0f,
            0f
        );

        lastCamPosition = cam.position;
    }
}
