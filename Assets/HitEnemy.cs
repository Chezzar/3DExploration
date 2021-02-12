using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    Collider MyCollider;
    bool OnAtack = false;
    float timeToAtack = 0f;

    private void Awake()
    {
        MyCollider = GetComponent<Collider>();
        timeToAtack = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !OnAtack)
        {

            MyCollider.enabled = true;

            OnAtack = true;
        }

        if (OnAtack)
        {

            TimeAtack();
        }
    }

    void TimeAtack()
    {
        timeToAtack += Time.deltaTime;

        if (timeToAtack >= 1f)
        {
            MyCollider.enabled = false;
            OnAtack = false;
            timeToAtack = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("golpeo");
            other.gameObject.SendMessage("LifeReduction", 1);
        }
    }
   
}
