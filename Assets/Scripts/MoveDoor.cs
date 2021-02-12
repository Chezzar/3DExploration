using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public Transform toMoveTo;
    public bool move = false;
    public float speed = 1.0f;
    public int numberOfTriggers;
    int a = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, toMoveTo.position, speed * Time.deltaTime);
        } 
    }

    public void activate()
    {
        move = true;
    }

    public void lookTo()
    {
        Debug.Log("LOOK TO");
        transform.LookAt(toMoveTo);
    }

    public void addAndMove()
    {
        a++;
        if(a == numberOfTriggers)
        {
            Debug.Log("Activate central");
            activate();
        }
    }
}
