using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private float speed = 5f;
    [Header("Map Bounds")]
    [SerializeField] private Vector3 xLimits = new Vector3(-20f, 0, 20f);
    [SerializeField] private Vector3 zLimits = new Vector3(-20f, 0, 20f);
    [SerializeField] private float limitOffset = 0.5f;
    private Vector3 moveInput;
    private Animator _animator;
    private Rigidbody _rb;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveInput = new Vector3(moveX, 0, moveZ);
        if (_animator != null)
        {
            _animator.SetFloat("speed", moveInput.sqrMagnitude);
        }

        Vector3 targetPos = transform.position + moveInput * speed * Time.fixedDeltaTime;
        targetPos = LimitPosition(targetPos);
        _rb.MovePosition(targetPos);
        if (moveInput != Vector3.zero)
        {
            transform.forward = moveInput;
        }
    }

    private Vector3 LimitPosition(Vector3 position)
    {
        float minX = xLimits.x + limitOffset;
        float maxX = xLimits.y - limitOffset;
        float minZ = zLimits.x + limitOffset;
        float maxZ = zLimits.y - limitOffset;
        float limitX = Mathf.Clamp(position.x, minX, maxX);
        float limitZ = Mathf.Clamp(position.z, minZ, maxZ);
        return new Vector3(limitX, position.y, limitZ);
    }
}
