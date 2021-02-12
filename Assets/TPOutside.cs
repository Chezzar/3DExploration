using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TPOutside : MonoBehaviour
{
    public AudioSource TempleAudio;
    public bool activateTemple;
    public AudioSource OverWorldAudio;
    public bool activateWorld;

    public AudioSource WaterFall;
    public Transform Dogo;
    public Transform Player;

    public Transform NewDogo;
    public Transform NewPlayer;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (activateWorld)
            {

                TempleAudio.enabled = false;
                OverWorldAudio.enabled = true;
                WaterFall.enabled = true;
            }

            else if (activateTemple) {

                TempleAudio.enabled = true;
                OverWorldAudio.enabled = false;
                WaterFall.enabled = false;
            }

            Dogo.GetComponent<NavMeshAgent>().enabled = false;
            Player.position = NewPlayer.position;
            Dogo.position = NewDogo.position;

            Dogo.GetComponent<NavMeshAgent>().enabled = true;

        }
    }
}
