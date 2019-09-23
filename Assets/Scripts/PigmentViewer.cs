using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PigmentViewer : Singleton<PigmentViewer>
{
    public Image lArrow;
    public TextMeshProUGUI lText;
    public Image rArrow;
    public TextMeshProUGUI rText;
    public Image pigmentImage;
    public TextMeshProUGUI pigmentText;

    // Start is called before the first frame update
    void Start()
    {
        lArrow.enabled = false;
        lText.enabled = false;
        rArrow.enabled = false;
        rText.enabled = false;
        pigmentImage.enabled = false;
        pigmentText.enabled = false;
    }

    public void ShowArrows()
    {
        lArrow.enabled = true;
        lText.enabled = true;
        rArrow.enabled = true;
        rText.enabled = true;
    }

    public void ChangeColor(Pigment pigment)
    {
        pigmentImage.enabled = true;
        pigmentText.enabled = true;
        pigmentImage.color = pigment.visualColor;
    }
}
