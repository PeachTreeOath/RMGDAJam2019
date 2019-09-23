using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : Singleton<ToggleManager>
{
    private List<ToggleableObject> toggleableObjects;

    // Start is called before the first frame update
    void Start()
    {
        ToggleableObject[] objects = GetComponentsInChildren<ToggleableObject>();
        toggleableObjects = new List<ToggleableObject>(objects);
    }

    public void TogglePigment(Pigment pigment)
    {
        foreach (ToggleableObject obj in toggleableObjects)
        {
            if (obj.pigmentColor.Equals(pigment.pigmentColor))
                obj.Toggle();
        }
    }
}
