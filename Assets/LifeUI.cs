using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public CapsuleMove MyLife;

    public RectTransform MyRectTransform;

    int Empty = 240;
    int totalLife;

    private void Start()
    {
        totalLife = MyLife.life;
    }

    private void Update()
    {
        //MyLife.life;

        int ToDraw = (MyLife.life * Empty)/totalLife;

        MyRectTransform.offsetMin = new Vector2(-ToDraw + 240,0 );
    }
}
