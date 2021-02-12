using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PuzzleEnemyManager : MonoBehaviour
{
    public int currtarg = -1;
    public List<MoveDoor> enemys;
    public List<GameObject> Plates;
    public Transform normaltarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTarget(int a)
    {
        if(currtarg >= 0 && currtarg < 3)
        {
            currtarg = a;
            enemys[currtarg].activate();
            enemys[currtarg].lookTo();
        }
    }
}
