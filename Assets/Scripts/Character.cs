using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int life;
    public int Life
    {

        get { return life; }
        set { life = value; }

    }

    private float speed;
    public float Speed
    {

        get { return speed; }
        set { speed = value; }

    }

    protected virtual void Move() { }

}
