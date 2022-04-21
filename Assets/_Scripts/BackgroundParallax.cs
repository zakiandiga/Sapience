using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    private float parallaxSpeedX;
    [SerializeField]
    private float parallaxSpeedY;
    
    private Transform cameraTransform;
    private float startPositionX, startPositionY;
    private float spriteSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        spriteSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float relativeDist = cameraTransform.position.x * parallaxSpeedX;
        float relativeDistY = cameraTransform.position.y * parallaxSpeedY;
        transform.position = new Vector3(startPositionX + relativeDist, startPositionY + relativeDistY, transform.position.z);

        // Loop Parallax Effect
        float relativeCameraDist = cameraTransform.position.x * (1 - parallaxSpeedX);
        if (relativeCameraDist > startPositionX + spriteSizeX)
        {
            startPositionX += spriteSizeX;
        }
        else if (relativeCameraDist < startPositionX - spriteSizeX)
        {
            startPositionX -= spriteSizeX;
        }
    }
}
