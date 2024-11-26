using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    private Animator animator;
    public Animator animatorgun;
    private Rigidbody rb;

    private float walkSpeed;
    public float runSpeed = 3f;
    private bool isRunning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        walkSpeed = speed;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D için giriş
        float vertical = Input.GetAxis("Vertical");     // W/S için giriş

        Vector3 movement = new Vector3(0f, 0f, vertical);

        // Koşma durumu (Shift tuşuna basıldığında)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = true;
            speed = runSpeed;
            animator.SetBool("run", true);
        }
        else
        {
            isRunning = false;
            speed = walkSpeed; 
            animator.SetBool("run", false);
        }

        // Karakteri hareket ettir
        if (movement.magnitude > 0.1f)
        {
            transform.Translate(movement.normalized * speed * Time.deltaTime, Space.Self);
            animator.SetBool("aim", true);
        }
        else
        {
            animator.SetBool("aim", false);
        }

        if (vertical < -0.1f) // Geri geri yürüme animasyonu icin yaptim
        {
            speed = 0.1f;
            animator.SetBool("turnback", true);
        }
        else
        {
            animator.SetBool("turnback", false);
        }


        // Karakteri sağa/sola döndür
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            float rotation = horizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotation, 0f);

            if (rotation > 0f) // Sağ dönüş
            {
                animator.SetBool("right", true);
                animator.SetBool("left", false);
            }
            else if (rotation < 0f) // Sol dönüş
            {
                animator.SetBool("left", true);
                animator.SetBool("right", false);
            }
        }
        else
        {
            animator.SetBool("right", false);
            animator.SetBool("left", false);
        }

        animator.SetFloat("Speed", movement.magnitude);
    }
}
