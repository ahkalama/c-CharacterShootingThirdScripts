using UnityEngine;

public class ShootingCharacter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private Transform characterBody;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private LayerMask aimColliderLayerMask;

    private bool isAiming = false;
    private float blend = 0f;
    private float verticalRotation = 0f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;

    private Animator anim;
    private bool aimrunning = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        blend = Mathf.Lerp(blend, isAiming ? 1f : 0f, Time.deltaTime * transitionSpeed);
        BlendCameras(blend);
        HandleRaycast();
        HandleShooting();
        HandleCharacterRotation();
    }

    private void HandleShooting()
    {
        if (isAiming)
        {
            anim.SetLayerWeight(1, 1f);
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 aimDir = (debugTransform.position - bulletSpawnPoint.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
                bullet.AddComponent<BulletMover>().Initialize(aimDir, bulletSpeed);
            }
        }
        else
        {
            anim.SetLayerWeight(1, 0f);
            anim.SetBool("aim", false);
        }
    }

    private void BlendCameras(float blend)
    {
        mainCamera.fieldOfView = Mathf.Lerp(60f, 30f, blend);
        aimCamera.fieldOfView = Mathf.Lerp(60f, 30f, blend);

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, aimCamera.transform.position, blend);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, aimCamera.transform.rotation, blend);
    }

    private void HandleRaycast()
    {
        float yOffset = isAiming ? 110 : 120;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2 + yOffset);

        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, aimColliderLayerMask))
            debugTransform.position = hit.point;
    }

    private void HandleCharacterRotation()
    {
        // Kamera yönüne göre karakterin y ekseninde dönmesini sağla
        Vector3 targetDirection = new Vector3(mainCamera.transform.forward.x, 0f, mainCamera.transform.forward.z); // Yalnızca yatay eksende dönmesini sağla
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Hedef rotayı al
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, targetRotation, Time.deltaTime * 10f); // Karakteri yavaşça döndür
        }
    }
}
