using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    public ObstacleSpawner Instance { get; private set; }

    [SerializeField] private ObstacleListSO obstacleListSO;
    [SerializeField] private Transform obstacleSpawnPoint;
    [SerializeField] private Transform objectParent;

    private ObstacleSO obstacleSO;

    private float obstacleSpawnTimer;
    private float obstacleSpawnTimerMax = 1f;
    private int obstacleSpawnCount = 0;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (GameManager.Instance.IsGamePlaying()) {
            obstacleSpawnTimer -= Time.deltaTime;
            if (obstacleSpawnTimer < 0) {
                obstacleSpawnTimer = obstacleSpawnTimerMax;

                obstacleSO = obstacleListSO.obstaclesSO[UnityEngine.Random.Range(0, obstacleListSO.obstaclesSO.Count)];
                SpawnObstacle();
                obstacleSpawnCount++;

                if (obstacleSpawnCount > 20) {
                    obstacleSpawnCount = 0;
                }
            }
        }
    }

    private void SpawnObstacle() {
        GameObject newObject = Instantiate(obstacleSO.prefab, obstacleSpawnPoint.position, transform.rotation, objectParent);
        newObject.name = "Obstacle_" + obstacleSpawnCount;
    }

}
