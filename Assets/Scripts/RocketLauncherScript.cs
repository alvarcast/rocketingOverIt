using UnityEngine;
using UnityEngine.InputSystem;

public class RocketLauncherScript : MonoBehaviour
{
    private Vector2 dir;
    private float angle;

    public void AimAt(Vector2 targetPos)
    {
        dir = targetPos - (Vector2)transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
