using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenEnemyTarget : MonoBehaviour
{
    public PuzzleEnemyManager enemyManager;
    public int myId;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerHidden " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            enemyManager.currtarg = myId;
        }
    }
}
