using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenFader.instance.StartIntro();
    }
}
