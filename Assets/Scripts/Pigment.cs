using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PigmentColor
{
    NONE,
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

    private void Awake()
    {
        switch (pigmentColor)
        {
            case PigmentColor.BLUE:
                visualColor = new Color(.22f, .40f, .71f);
                break;
            case PigmentColor.YELLOW:
                visualColor = new Color(.88f, .83f, .39f);
                break;
            case PigmentColor.RED:
                visualColor = new Color(.86f, .31f, .30f);
                break;
            default:
                visualColor = GetComponent<SpriteRenderer>().color;
                break;
        }
        //visualColor = GetComponent<SpriteRenderer>().color;
    }

    public Pigment PickupColor()
    {
        gameObject.SetActive(false);
        return this;
    }
}
