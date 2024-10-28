using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    public static ObstacleSpawner Instance { get; private set; }

    [SerializeField] private ObstacleListSO obstacleListSO;
    [SerializeField] private Transform obstacleSpawnPoint;
    [SerializeField] private Transform objectParent;

    private Dictionary<ObstacleSO, Queue<GameObject>> obstaclePool = new Dictionary<ObstacleSO, Queue<GameObject>>();

    private float obstacleSpawnTimer;
    private float obstacleSpawnTimerMax = 1f;
    private int obstacleSpawnCount = 0;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("There is more than one Instance of ObstacleSpawner");
            Destroy(gameObject);
        }

        foreach (ObstacleSO obstacleSO in obstacleListSO.obstaclesSO) {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            obstaclePool.Add(obstacleSO, poolQueue);
        }
    }

    private void Update() {
        if (GameManager.Instance.IsGamePlaying()) {
            obstacleSpawnTimer -= Time.deltaTime;
            if (obstacleSpawnTimer < 0) {
                obstacleSpawnTimer = obstacleSpawnTimerMax;
                ObstacleSO randomObstacleSO = obstacleListSO.obstaclesSO[Random.Range(0, obstacleListSO.obstaclesSO.Count)];
                SpawnObstacle(randomObstacleSO);
                obstacleSpawnCount++;

                if (obstacleSpawnCount > 20) {
                    obstacleSpawnCount = 0;
                }
            }
        }
    }

    private void SpawnObstacle(ObstacleSO obstacleSO) {
        GameObject obstacle = GetPoolObstacle(obstacleSO);
        obstacle.transform.position = obstacleSpawnPoint.position;
        obstacle.transform.rotation = Quaternion.identity;
        obstacle.transform.SetParent(objectParent);
        obstacle.SetActive(true);
        obstacle.name = "Obstacle_" + obstacleSpawnCount;
    }

    private GameObject GetPoolObstacle(ObstacleSO obstacleSO) {
        if (obstaclePool[obstacleSO].Count > 0) {
            GameObject pooledObstacle = obstaclePool[obstacleSO].Dequeue();
            return pooledObstacle;
        }

        GameObject newObstacle = Instantiate(obstacleSO.prefab, obstacleSpawnPoint.position, Quaternion.identity, objectParent);
        obstaclePool[obstacleSO].Enqueue(newObstacle);
        return newObstacle;

    }

    public void ReturnObstacleToPool(GameObject obstacle, ObstacleSO obstacleSO) {
        obstacle.SetActive(false);
        obstacle.transform.SetParent(null);
        obstaclePool[obstacleSO].Enqueue(obstacle);
    }
}
