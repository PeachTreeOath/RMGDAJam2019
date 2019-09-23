using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableObject : MonoBehaviour
{
    public bool isToggled;
    public PigmentColor pigmentColor;

    private SpriteRenderer spriteRenderer;
    private Color origColor;
    private Color neutralColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        origColor = GetComponent<SpriteRenderer>().color;
        neutralColor = new Color(.5f, .5f, .5f, .5f);

        if (!isToggled)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            spriteRenderer.color = neutralColor;
        }
    }

    public void Toggle()
    {
        if (isToggled)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            spriteRenderer.color = neutralColor;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            spriteRenderer.color = origColor;
        }
        isToggled = !isToggled;
    }
}
