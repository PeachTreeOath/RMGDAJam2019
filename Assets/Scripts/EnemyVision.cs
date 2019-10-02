using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    public EnemyController enemy; // Inspector assigned reference to parent

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            Debug.Log("DETECTED");
            enemy.DetectedPlayer();
        }
    }
}
