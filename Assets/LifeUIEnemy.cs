using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIEnemy : MonoBehaviour
{
    public Enemy MyLife;
    public RectTransform MyRectTransform;

    float Empty = 0.9f;
    int totalLife;


    private void Start()
    {
        totalLife = MyLife.life;
    }
    // Update is called once per frame
    void Update()
    {
        float ToDraw = (MyLife.life * Empty) / totalLife;

        MyRectTransform.offsetMin = new Vector2(-ToDraw + 0.9f, 0);

    }
}
