using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : Singleton<ScreenFader>
{
    public CanvasGroup group;

    private bool isOutroing;
    private bool isIntroing;
    private float outroStartTime;

    private void Start()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOutroing)
        {
            float alpha = Mathf.Lerp(0, 1, ((Time.time-1) - outroStartTime) / 2) ;
            group.alpha = alpha;
            group.blocksRaycasts = true;
            group.interactable = true;
        }
        else if (isIntroing)
        {
            float alpha = Mathf.Lerp(0, 1, ((Time.time-1) - outroStartTime) / 2) ;
            group.alpha = alpha;
            group.blocksRaycasts = true;
            group.interactable = true;
        }
    }

    public void StartOutro()
    {
        outroStartTime = Time.time;
        isOutroing = true;
        group.alpha = 0;
    }

    public void StartIntro()
    {
        outroStartTime = Time.time;
        isIntroing = true;
        group.alpha = 0;
    }
}
