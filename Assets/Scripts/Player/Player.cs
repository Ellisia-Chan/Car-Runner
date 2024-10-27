using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float dashSpeedMultiplier = 3f;
    [SerializeField] private float rotationSpeed = 50f;

    private Rigidbody rb;
    private Vector2 movement;

    private bool isObstacleHit = false;
    private bool isDashing = false;
    private bool inConfinerCollider = false;
    private float dashDuration = 0.1f;
    private float dashCD = 1f;
    private float defaultMoveSpeed;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody>();

        defaultMoveSpeed = moveSpeed;
    }

    private void Start() {
        GameInput.Instance.OnDashAction += GameInput_OnDashAction;
    }

    private void GameInput_OnDashAction(object sender, System.EventArgs e) {
        HandleDash();
    }

    private void Update() {
        PlayerInputActions();
    }

    private void FixedUpdate() {
        if (GameManager.Instance.IsGamePlaying()) {
            HandleMovement();
            RotatePLayer();
        }
    }

    private void PlayerInputActions() {
        movement = GameInput.Instance.GetMovementVectorNormalized();
    }

    private void HandleMovement() {
        Vector3 movement3D = new Vector3(movement.x, 0, movement.y);
        rb.velocity = new Vector3(movement3D.x * moveSpeed, rb.velocity.y, movement3D.z * moveSpeed);
    }

    private void HandleDash() {
        if (!isDashing && movement.x != 0) {
            isDashing = true;
            moveSpeed *= dashSpeedMultiplier;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash() {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = defaultMoveSpeed;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.Contains("Collider")) {
            inConfinerCollider = true;

            if (movement.x != 0) {
                ResetRotation();
            }
        }

        if (collision.gameObject.GetComponent<ObstacleMovement>() != null) {
            isObstacleHit = true;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.name.Contains("Collider")) {
            inConfinerCollider = false;
        }
    }

    private void RotatePLayer() {
        float targetYRotation = 0f;

        if (movement.x > 0) {
            targetYRotation = 20f;
        } else {
            targetYRotation = -20f;
        }

        if (movement.x != 0 && !inConfinerCollider) {
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } else {
            ResetRotation();
        }

    }

    private void ResetRotation() {
        Quaternion resetRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, resetRotation, rotationSpeed * Time.deltaTime);
    }

    public bool IsObstacleHit() {
        return isObstacleHit;
    }

    private void OnDestroy() {
        GameInput.Instance.OnDashAction -= GameInput_OnDashAction;
    }
}
