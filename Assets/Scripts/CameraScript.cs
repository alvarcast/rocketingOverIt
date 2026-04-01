using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offsetY;

    private float fixedX;
    private float fixedZ;

    void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;

        offsetY = transform.position.y - player.position.y;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(fixedX, player.position.y + offsetY, fixedZ);
    }
}
