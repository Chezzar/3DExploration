using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : MonoBehaviour
{
    public List<PressurePlate> MyListToActivate = new List<PressurePlate>();

    public bool activateDoor;

    public int plates;

    public Transform toGo;

    public AudioSource Asource;

    private void Awake()
    {
        Asource = GetComponent<AudioSource>();

        foreach(var i in MyListToActivate) {

            i.MyDoor += ActivatePlate;
        }
    }

    void ActivatePlate() {

        plates++;

        if (plates == MyListToActivate.Capacity) {

            this.transform.position = new Vector3(transform.position.x,transform.position.y + 6,transform.position.z);
            Asource.Play();
        }
    }

    private void OnDestroy()
    {
        foreach (var i in MyListToActivate)
        {

            i.MyDoor -= ActivatePlate;
        }
    }
}
