using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    [SerializeField] private VfxCreator vfxCreator;

    private void Start()
    {
        vfxCreator = FindObjectOfType<VfxCreator>();
    }

    public void Initialize(Vector3 direction, float bulletSpeed)
    {
        moveDirection = direction;
        speed = bulletSpeed;
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
{
    if (this != null)
    {
        vfxCreator.TriggerVFX(transform.position);
        Destroy(gameObject);
    }
}

}
