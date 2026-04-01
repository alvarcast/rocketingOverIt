using UnityEngine;

public class IceScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PhysicsMaterial2D normalFriction;
    [SerializeField] private PhysicsMaterial2D iceFriction;

    private CapsuleCollider2D cc;

    private void Start()
    {
        cc = player.GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cc.sharedMaterial = iceFriction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cc.sharedMaterial = normalFriction;
        }
    }
}
