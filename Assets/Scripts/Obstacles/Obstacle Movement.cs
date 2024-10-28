using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour {

    private float obstacleSpeed = 100f;
    private float speedDeclerationRate = 70f;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (GameManager.Instance.IsGamePlaying()) {
            rb.velocity = new Vector3(0, 0, -obstacleSpeed);
        } else {
            if (obstacleSpeed > 0) {
                obstacleSpeed -= speedDeclerationRate * Time.deltaTime;
            }

            obstacleSpeed = Mathf.Max(obstacleSpeed, 0);
            rb.velocity = new Vector3 (0, 0, -obstacleSpeed);
        }
    }

    public void IncreaseObstacleSpeed() {
        obstacleSpeed += 10f;
    }
}