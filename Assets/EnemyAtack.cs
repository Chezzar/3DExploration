using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
    public Enemy enemy;

    private void Awake()
    {
        enemy = transform.root.GetComponent<Enemy>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Player") {
        //    Debug.Log("golpeo enemigo");
        //    other.gameObject.SendMessage("Damage", enemy.Damage);
        //}
    }
}
