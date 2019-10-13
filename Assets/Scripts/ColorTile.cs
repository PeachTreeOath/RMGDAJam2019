using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    public Material bnwMat;
    public bool isColor;

    private SpriteRenderer sprite;
    private Material defaultMat;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        defaultMat = sprite.material;
        SwitchToBnW();
    }

    public void SwitchToColor()
    {
        isColor = true;
        sprite.material = defaultMat;
    }

    public void SwitchToBnW()
    {
        isColor = false;
        sprite.material = bnwMat;
    }
}
