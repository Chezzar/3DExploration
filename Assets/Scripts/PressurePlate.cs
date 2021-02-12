using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{

    public delegate void Activate();

    public event Activate MyDoor;

    public bool IsAcyivated = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Dogo" || other.gameObject.tag == "Enemy")
        {
            if (!IsAcyivated)
            {
                MyDoor();
                IsAcyivated = true;
            }
        }
    }

    //public UnityEvent toActivate;

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Trigger " + other.gameObject.tag);
    //    if (other.gameObject.tag == "Player")
    //    {
    //        toActivate.Invoke();
    //    }
    //}
}
