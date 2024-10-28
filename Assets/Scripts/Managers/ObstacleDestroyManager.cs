using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyManager : MonoBehaviour {

    //=================================================
    //
    // Description: Destroy the parent of the gameObject
    //
    // return: void
    //
    // ================================================

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.GetComponent<Player>()) {
            ObstacleSORef obstacleComponent = other.gameObject.GetComponent<ObstacleSORef>();

            if (obstacleComponent != null) {
                ObstacleSO obstacleSO = obstacleComponent.obstacleSO;

                if (obstacleSO != null)
                {
                    ObstacleSpawner.Instance.ReturnObstacleToPool(other.transform.gameObject, obstacleSO);
                }
            }
        }
    }
}
