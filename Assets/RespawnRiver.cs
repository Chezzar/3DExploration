using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRiver : MonoBehaviour
{
    public List<Transform> RespawnPlaces;
    public List<Transform> RespawnPlacesDogo;

    public Transform ToRespawn;
    public Transform ToRespawnDogo;
    public GameObject Dogo;

    public float winnerPlace = 200;

    public int elementToPlace = 0;

    private void Start()
    {
        Dogo = GameObject.FindGameObjectWithTag("Dogo");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Mode2(other);
        }
    }

    private void Mode1(Collider other)
    {

        other.transform.position = ToRespawn.position;
        Dogo.transform.position = ToRespawnDogo.position;

    }

    private void Mode2(Collider other)
    {
        winnerPlace = 200;

        elementToPlace = 0;

        int checkPlace = 0;

        foreach (var i in RespawnPlaces)
        {

            float actualCheckPosition = Mathf.Abs(Vector2.Distance(new Vector2(i.position.x,i.position.z),new Vector2(other.transform.position.x, other.transform.position.z)));

            if (actualCheckPosition < winnerPlace)
            {

                elementToPlace = checkPlace;
                winnerPlace = actualCheckPosition;
            }

            checkPlace++;
        }

        other.transform.position = RespawnPlaces[elementToPlace].transform.position;
        Dogo.transform.position = RespawnPlacesDogo[elementToPlace].transform.position;
    }
}
