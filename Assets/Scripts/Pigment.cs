using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PigmentColor
{
    BLUE,
    YELLOW,
    RED,
    GREEN,
    ORANGE,
    PURPLE
}

public class Pigment : MonoBehaviour
{

    public PigmentColor pigmentColor;
    public Color visualColor;

    private void Start()
    {
        visualColor = GetComponent<SpriteRenderer>().color;
    }

    public Pigment PickupColor()
    {
        gameObject.SetActive(false);
        return this;
    }
}
