using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    public EnemyController enemy; // Inspector assigned reference to parent

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            enemy.DetectedPlayer(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            enemy.DetectedPlayer(false);
        }
    }

    public void PlayerStealthed()
    {
        enemy.DetectedPlayer(false);
    }
}
