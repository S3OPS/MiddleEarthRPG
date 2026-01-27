using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    public float MoveSpeed = 5.5f;
    public float SprintMultiplier = 1.6f;
    public float JumpForce = 5f;

    private Rigidbody _rigidbody;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        var speed = MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= SprintMultiplier;
        }

        var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var direction = transform.TransformDirection(input.normalized);
        var velocity = new Vector3(direction.x * speed, _rigidbody.velocity.y, direction.z * speed);
        _rigidbody.velocity = velocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            _isGrounded = true;
        }
    }
}