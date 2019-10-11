using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem
{
    public RectTransform menuObject;
    public Pigment pigment;
    public float angle;
}

public class CycleMenu : Singleton<CycleMenu>
{

    public float radius;
    public float spinSpeed;
    public List<RectTransform> pigmentUIObjects = new List<RectTransform>(); // Fill this in inspector
    private List<MenuItem> menuItems = new List<MenuItem>();

    private float currNorthAngle;
    private float prevAngle;
    private float destAngle;
    private float spinStartTime;

    // Start is called before the first frame update
    void Start()
    {
        currNorthAngle = 90;
        destAngle = currNorthAngle;
        prevAngle = currNorthAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (destAngle != currNorthAngle)
        {
            currNorthAngle = Mathf.Lerp(prevAngle, destAngle, (Time.time - spinStartTime) * spinSpeed);
            SetAngles();
            MoveItemsToPositions();
        }
    }

    private void MoveItemsToPositions()
    {
        foreach (MenuItem menuItem in menuItems)
        {
            menuItem.menuObject.localPosition = new Vector2(Mathf.Cos(menuItem.angle * Mathf.Deg2Rad), Mathf.Sin(menuItem.angle * Mathf.Deg2Rad)) * radius;
        }
    }

    private void SetAngles()
    {
        int i = 0;
        foreach (MenuItem menuItem in menuItems)
        {
            menuItem.angle = currNorthAngle + i;
            float angleToSpin = 360 / menuItems.Count;
            i += (int)angleToSpin;
        }
    }

    public void SpinCCW(int pigmentIndex)
    {
        spinStartTime = Time.time;
        prevAngle = destAngle;
        currNorthAngle = destAngle;
        float angleToSpin = 360 / menuItems.Count;
        destAngle = currNorthAngle - angleToSpin;
    }

    public void SpinCW(int pigmentIndex)
    {
        spinStartTime = Time.time;
        prevAngle = destAngle;
        currNorthAngle = destAngle;
        float angleToSpin = 360 / menuItems.Count;
        destAngle = currNorthAngle + angleToSpin;

        for (int i = 0; i < menuItems.Count; i++)
        {

        }
    }

    public void AddToMenu(Pigment pigment)
    {
        MenuItem item = new MenuItem();
        item.menuObject = pigmentUIObjects[menuItems.Count]; // Grab the next available gameobject and use this
        Image img = pigmentUIObjects[menuItems.Count].GetComponent<Image>();
        img.enabled = true;
        img.color = pigment.visualColor;
        item.pigment = pigment;
        menuItems.Add(item);

        float angleToSpin = 360 / menuItems.Count;
        currNorthAngle = angleToSpin + 90;
        destAngle = currNorthAngle;

        SetAngles();
        MoveItemsToPositions();
    }
}
