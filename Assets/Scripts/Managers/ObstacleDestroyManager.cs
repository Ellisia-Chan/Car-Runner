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
            Destroy(other.transform.parent.gameObject);
        }
    }
}
