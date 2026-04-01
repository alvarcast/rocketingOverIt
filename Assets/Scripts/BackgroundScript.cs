using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private float startPos;
    private float length;

    private float distance;
    private float movement;

    void Start()
    {
        startPos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        distance = cam.transform.position.y * parallaxEffect;
        movement = cam.transform.position.y * (1 - parallaxEffect);

        transform.position = new Vector2 (transform.position.x, startPos + distance);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
